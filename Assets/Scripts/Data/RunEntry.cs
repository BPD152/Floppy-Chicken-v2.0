using System;

/// <summary>
/// Thong tin cua mot luot choi da ket thuc.
/// SaveData giu List<RunEntry> gom 10 luot gan nhat.
/// RunPopup hien thi list nay ra UI.
/// </summary>
[Serializable]
public class RunEntry
{
    public int score;        // So diem dat duoc trong luot nay
    public float playTime;   // Thoi luong choi (giay), tinh tu khi state chuyen sang Playing
    public string dateTime;  // Thoi diem ket thuc luot, format ISO 8601 ("2026-07-19T14:30:00")
}