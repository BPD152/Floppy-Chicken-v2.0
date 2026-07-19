using System;
using System.Collections.Generic;

/// <summary>
/// Toan bo du lieu duoc luu xuong file save.json.
/// SaveManager serialize class nay thanh JSON de ghi, va deserialize khi doc.
/// Day la "1 khoi duy nhat" dai dien cho trang thai luu cua game.
/// </summary>
[Serializable]
public class SaveData
{
    // --- High score ---
    public int highScore = 0;

    // --- Lich su choi ---
    // 10 luot gan nhat. Luot moi nhat them vao, vuot 10 thi bo luot cu nhat.
    public List<RunEntry> records = new List<RunEntry>();

    // --- Cai dat am luong (0.0 -> 1.0) ---
    // Mac dinh de 1 (max) cho lan dau mo app khi chua co save.
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public float ambientVolume = 1f;
}