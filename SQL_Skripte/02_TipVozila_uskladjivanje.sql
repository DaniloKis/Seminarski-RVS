/*
        sifra "01" (Kombi)  -> 30 kg
        sifra "02" (Kamion) -> 1000 kg
*/
USE KurirskaSluzba2026;
GO

-- Kombi (standardno vozilo) - sifra 01
IF NOT EXISTS (SELECT 1 FROM dbo.TipVozila WHERE Naziv = 'Kombi')
BEGIN
    INSERT INTO dbo.TipVozila (Sifra, Naziv) VALUES ('01', 'Kombi');
END
GO

-- Kamion (posebno vozilo) - sifra 02
IF NOT EXISTS (SELECT 1 FROM dbo.TipVozila WHERE Naziv = 'Kamion')
BEGIN
    INSERT INTO dbo.TipVozila (Sifra, Naziv) VALUES ('02', 'Kamion');
END
GO

SELECT Sifra, Naziv FROM dbo.TipVozila;
GO
