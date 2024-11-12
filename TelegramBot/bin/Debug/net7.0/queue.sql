--
-- ‘‡ÈÎ Ò„ÂÌÂËÓ‚‡Ì Ò ÔÓÏÓ˘¸˛ SQLiteStudio v3.4.4 ‚ Mon Nov 11 11:50:22 2024
--
-- »ÒÔÓÎ¸ÁÓ‚‡ÌÌ‡ˇ ÍÓ‰ËÓ‚Í‡ ÚÂÍÒÚ‡: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- “‡·ÎËˆ‡: Queue_¿ “»Œ—
CREATE TABLE IF NOT EXISTS Queue_¿ “»Œ— (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    UserId INTEGER UNIQUE);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (1, 'renuxela', 1041411876);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (2, 'faysonin', 827919637);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (3, 'Satori12', 1134073796);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (4, '¿ÎÂÍÒ‡Ì‰ ÿÂ‚˜ÂÌÍÓ', 5540649849);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (5, 'ilya2005pan4', 1009510323);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (6, 'ZimaBlue9', 1955527569);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (7, 'werty2648', 1396730464);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (8, 'verretta', 1296088366);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (9, 'S1024', 884211123);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (10, 'push_offset_name', 845340305);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (11, 'lutatel_risa', 2078365425);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (12, 'Daniil_Rudenya', 1251070423);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (13, 'fluffy11lol', 640411109);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (14, 'ivan_hdjddjs', 563239661);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (15, 'v4ssal', 865601041);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (16, 'TPNE05', 835559951);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (17, 'egor_geekin', 894871193);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (18, 'xxxSkibidiSigmaxxx', 5167248295);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (19, 'koan6gi', 1464860402);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (20, 'alekRadt', 824298179);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (21, 'kostyabelbet', 1256601598);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (22, 'DJ_4_7', 525033481);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (23, 'valery_kuzh', 947840361);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (24, 'who_o', 1006764486);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (25, 'leravvvvvvvv', 1192276306);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (26, 'gaegxh', 988522114);
INSERT INTO Queue_¿ “»Œ— (ID, Name, UserId) VALUES (27, 'SergeyOrsik', 1232957794);

-- “‡·ÎËˆ‡: Queue_–œ»
CREATE TABLE IF NOT EXISTS Queue_–œ» (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    UserId INTEGER UNIQUE);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (1, 'lutatel_risa', 2078365425);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (2, 'ivan_hdjddjs', 563239661);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (3, '¿ÎÂÍÒ‡Ì‰ ÿÂ‚˜ÂÌÍÓ', 5540649849);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (4, 'gaegxh', 988522114);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (5, 'xxxSkibidiSigmaxxx', 5167248295);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (6, 'Satori12', 1134073796);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (7, 'kostyabelbet', 1256601598);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (8, 'Daniil_Rudenya', 1251070423);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (9, 'TPNE05', 835559951);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (10, 'temaqt', 885887534);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (11, 'faysonin', 827919637);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (12, 'renuxela', 1041411876);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (13, 'ilya2005pan4', 1009510323);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (14, 'ZimaBlue9', 1955527569);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (15, 'leravvvvvvvv', 1192276306);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (16, 'fluffy11lol', 640411109);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (17, 'who_o', 1006764486);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (18, 'werty2648', 1396730464);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (19, 'v4ssal', 865601041);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (20, 'alekRadt', 824298179);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (21, 'verretta', 1296088366);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (22, 'SergeyOrsik', 1232957794);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (23, 'DJ_4_7', 525033481);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (24, 'push_offset_name', 845340305);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (25, 'egor_geekin', 894871193);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (26, 'koan6gi', 1464860402);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (27, 'S1024', 884211123);
INSERT INTO Queue_–œ» (ID, Name, UserId) VALUES (28, 'valery_kuzh', 947840361);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
