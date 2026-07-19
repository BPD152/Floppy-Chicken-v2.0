using System.IO;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quan ly doc/ghi du lieu game xuong file save.json.
/// Ghi ngay moi khi co thay doi (an toan neu app crash).
/// Luu 3 thu: high score, 10 luot choi gan nhat, cai dat am luong.
/// </summary>
public class SaveManager : Singleton<SaveManager>
{
    // Du lieu game dang giu trong bo nho.
    // Doc file 1 lan vao day, moi thao tac lam tren _data, roi ghi xuong file.
    private SaveData _data;

    // Duong dan file save tren may.
    // Application.persistentDataPath: thu muc Unity danh rieng cho app, ton tai qua cac lan mo.
    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    protected override void Awake()
    {
        base.Awake();   // BAT BUOC: goi logic Singleton (gan Instance, chong trung)
        Load();         // Doc file ngay khi khoi dong
    }

    // ==================== DOC / GHI FILE ====================

    private void Load()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            _data = JsonUtility.FromJson<SaveData>(json);

            // Phong truong hop file ton tai nhung rong/hong -> _data ra null
            if (_data == null)
                _data = new SaveData();
        }
        else
        {
            // Lan dau mo app: chua co file -> tao du lieu mac dinh
            _data = new SaveData();
        }
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(_data, true);  // true = format dep, de doc
        File.WriteAllText(SavePath, json);
    }

    // ==================== API: RECORDS + HIGH SCORE ====================

    /// <summary>
    /// Goi khi Bird chet. Luu 1 luot choi, cap nhat high score neu vuot.
    /// </summary>
    public void SaveRun(int score, float playTime)
    {
        // 1. Tao ban ghi cho luot vua roi
        RunEntry entry = new RunEntry();
        entry.score = score;
        entry.playTime = playTime;
        entry.dateTime = System.DateTime.Now.ToString("o");  // ISO 8601

        // 2. Chen vao dau list -> luot moi nhat len tren cung
        _data.records.Insert(0, entry);

        // 3. Chi giu 10 luot gan nhat, bo bot luot cu
        if (_data.records.Count > 10)
            _data.records.RemoveRange(10, _data.records.Count - 10);

        // 4. Cap nhat high score neu vuot ky luc (chi tang, khong bao gio tut)
        if (score > _data.highScore)
            _data.highScore = score;

        // 5. Ghi xuong file ngay
        Save();
    }

    /// <summary> RunPopup goi de hien thi lich su choi. </summary>
    public List<RunEntry> GetRecords()
    {
        return _data.records;
    }

    /// <summary> Home Scene va Game Over popup doc de hien thi ky luc. </summary>
    public int HighScore => _data.highScore;

    // ==================== API: SETTINGS (VOLUME) ====================

    /// <summary>
    /// Goi khi nguoi choi chinh volume trong popup Setting.
    /// SoundManager goi ham nay de luu 3 muc am luong.
    /// </summary>
    public void SaveSettings(float music, float sfx, float ambient)
    {
        _data.musicVolume = music;
        _data.sfxVolume = sfx;
        _data.ambientVolume = ambient;
        Save();
    }

    /// <summary>
    /// SoundManager goi luc khoi dong de doc lai volume da luu.
    /// Tra ve 3 gia tri qua tham so 'out' (khong can tao class rieng).
    /// </summary>
    public void LoadSettings(out float music, out float sfx, out float ambient)
    {
        music = _data.musicVolume;
        sfx = _data.sfxVolume;
        ambient = _data.ambientVolume;
    }
}