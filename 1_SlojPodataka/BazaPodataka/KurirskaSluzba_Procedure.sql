
USE [KurirskaSluzba2026]
GO



IF OBJECT_ID('DajSvaVozila','P') IS NOT NULL DROP PROCEDURE [DajSvaVozila]
GO
CREATE PROCEDURE [DajSvaVozila]
AS
    SELECT * FROM TipVozila
GO

IF OBJECT_ID('DajVoziloPoNazivu','P') IS NOT NULL DROP PROCEDURE [DajVoziloPoNazivu]
GO
CREATE PROCEDURE [DajVoziloPoNazivu]
( @NazivVozila nvarchar(40) )
AS
    SELECT * FROM TipVozila WHERE TipVozila.Naziv = @NazivVozila
GO

IF OBJECT_ID('DajVoziloPoSifri','P') IS NOT NULL DROP PROCEDURE [DajVoziloPoSifri]
GO
CREATE PROCEDURE [DajVoziloPoSifri]
( @SifraVozila nvarchar(3) )
AS
    SELECT * FROM TipVozila WHERE TipVozila.Sifra = @SifraVozila
GO

IF OBJECT_ID('DodajNovoVozilo','P') IS NOT NULL DROP PROCEDURE [DodajNovoVozilo]
GO
CREATE PROCEDURE [DodajNovoVozilo]
( @Sifra nvarchar(3), @Naziv nvarchar(40) )
AS
    INSERT INTO TipVozila (Sifra, Naziv) VALUES (@Sifra, @Naziv)
GO

IF OBJECT_ID('IzmeniVozilo','P') IS NOT NULL DROP PROCEDURE [IzmeniVozilo]
GO
CREATE PROCEDURE [IzmeniVozilo]
( @StaraSifra nvarchar(3), @Sifra nvarchar(3), @Naziv nvarchar(40) )
AS
    UPDATE TipVozila SET Sifra = @Sifra, Naziv = @Naziv WHERE Sifra = @StaraSifra
GO

IF OBJECT_ID('ObrisiVozilo','P') IS NOT NULL DROP PROCEDURE [ObrisiVozilo]
GO
CREATE PROCEDURE [ObrisiVozilo]
( @Sifra nvarchar(3) )
AS
    DELETE FROM TipVozila WHERE Sifra = @Sifra
GO


IF OBJECT_ID('DajSvePaketeSaJoin','P') IS NOT NULL DROP PROCEDURE [DajSvePaketeSaJoin]
GO
CREATE PROCEDURE [DajSvePaketeSaJoin]
AS
    SELECT  P.KodPaketa, P.PosiljalacIme, P.PrimalacIme, P.PrimalacGrad,
            P.Tezina, P.OznakaUpozorenja, P.Cena, V.Naziv AS NazivVozila
    FROM Paket P INNER JOIN TipVozila V ON P.IDTipaVozila = V.Sifra
    ORDER BY P.Tezina DESC
GO

IF OBJECT_ID('DajSvePaketeSaJoinSifromVozila','P') IS NOT NULL DROP PROCEDURE [DajSvePaketeSaJoinSifromVozila]
GO
CREATE PROCEDURE [DajSvePaketeSaJoinSifromVozila]
AS
    SELECT  P.KodPaketa, P.PosiljalacIme, P.PrimalacIme, V.Naziv AS NazivVozila,
            V.Sifra AS SifraVozila
    FROM Paket P INNER JOIN TipVozila V ON P.IDTipaVozila = V.Sifra
GO

IF OBJECT_ID('DajPaketPoPrimaocu','P') IS NOT NULL DROP PROCEDURE [DajPaketPoPrimaocu]
GO
CREATE PROCEDURE [DajPaketPoPrimaocu]
( @PaketPrimalac nvarchar(60) )
AS
    SELECT  P.KodPaketa, P.PosiljalacIme, P.PrimalacIme, P.PrimalacGrad,
            P.Tezina, P.OznakaUpozorenja, P.Cena, V.Naziv AS NazivVozila
    FROM Paket P INNER JOIN TipVozila V ON P.IDTipaVozila = V.Sifra
    WHERE P.PrimalacIme LIKE '%' + @PaketPrimalac + '%'
    ORDER BY P.Tezina DESC
GO

IF OBJECT_ID('DajPaketPoKodu','P') IS NOT NULL DROP PROCEDURE [DajPaketPoKodu]
GO
CREATE PROCEDURE [DajPaketPoKodu]
( @PaketKod nvarchar(13) )
AS
    SELECT * FROM Paket WHERE Paket.KodPaketa = @PaketKod
GO

IF OBJECT_ID('DajPaketPoKoduFilter','P') IS NOT NULL DROP PROCEDURE [DajPaketPoKoduFilter]
GO
CREATE PROCEDURE [DajPaketPoKoduFilter]
( @PaketKod nvarchar(13) )
AS
    SELECT  P.KodPaketa, P.PosiljalacIme, P.PrimalacIme, P.PrimalacGrad,
            P.Tezina, P.OznakaUpozorenja, P.Cena, V.Naziv AS NazivVozila
    FROM Paket P INNER JOIN TipVozila V ON P.IDTipaVozila = V.Sifra
    WHERE P.KodPaketa LIKE '%' + @PaketKod + '%'
    ORDER BY P.Tezina DESC
GO

IF OBJECT_ID('DajBrojPaketaPremaSifriVozila','P') IS NOT NULL DROP PROCEDURE [DajBrojPaketaPremaSifriVozila]
GO
CREATE PROCEDURE [DajBrojPaketaPremaSifriVozila]
( @SifraVozila nvarchar(3) )
AS
    SELECT COUNT(*) AS ukupno FROM Paket WHERE Paket.IDTipaVozila = @SifraVozila
GO

IF OBJECT_ID('DodajNoviPaket','P') IS NOT NULL DROP PROCEDURE [DodajNoviPaket]
GO
CREATE PROCEDURE [DodajNoviPaket]
( @KodPaketa nvarchar(13),
  @PosiljalacIme nvarchar(60), @PosiljalacAdresa nvarchar(80), @PosiljalacGrad nvarchar(40),
  @PrimalacIme nvarchar(60), @PrimalacAdresa nvarchar(80), @PrimalacPostanskiBroj nvarchar(10), @PrimalacGrad nvarchar(40),
  @Tezina decimal(10,2), @OznakaUpozorenja nvarchar(40), @Cena decimal(10,2), @IDTipaVozila nvarchar(3) )
AS
    INSERT INTO Paket (KodPaketa, PosiljalacIme, PosiljalacAdresa, PosiljalacGrad,
                       PrimalacIme, PrimalacAdresa, PrimalacPostanskiBroj, PrimalacGrad,
                       Tezina, OznakaUpozorenja, Cena, IDTipaVozila)
    VALUES (@KodPaketa, @PosiljalacIme, @PosiljalacAdresa, @PosiljalacGrad,
            @PrimalacIme, @PrimalacAdresa, @PrimalacPostanskiBroj, @PrimalacGrad,
            @Tezina, @OznakaUpozorenja, @Cena, @IDTipaVozila)
GO

IF OBJECT_ID('ObrisiPaket','P') IS NOT NULL DROP PROCEDURE [ObrisiPaket]
GO
CREATE PROCEDURE [ObrisiPaket]
( @KodPaketa nvarchar(13) )
AS
    DELETE FROM Paket WHERE Paket.KodPaketa = @KodPaketa
GO

IF OBJECT_ID('IzmeniPaket','P') IS NOT NULL DROP PROCEDURE [IzmeniPaket]
GO
CREATE PROCEDURE [IzmeniPaket]
( @StariKod nvarchar(13),
  @KodPaketa nvarchar(13),
  @PosiljalacIme nvarchar(60), @PosiljalacAdresa nvarchar(80), @PosiljalacGrad nvarchar(40),
  @PrimalacIme nvarchar(60), @PrimalacAdresa nvarchar(80), @PrimalacPostanskiBroj nvarchar(10), @PrimalacGrad nvarchar(40),
  @Tezina decimal(10,2), @OznakaUpozorenja nvarchar(40), @Cena decimal(10,2), @IDTipaVozila nvarchar(3) )
AS
    UPDATE Paket SET
        KodPaketa = @KodPaketa,
        PosiljalacIme = @PosiljalacIme, PosiljalacAdresa = @PosiljalacAdresa, PosiljalacGrad = @PosiljalacGrad,
        PrimalacIme = @PrimalacIme, PrimalacAdresa = @PrimalacAdresa, PrimalacPostanskiBroj = @PrimalacPostanskiBroj, PrimalacGrad = @PrimalacGrad,
        Tezina = @Tezina, OznakaUpozorenja = @OznakaUpozorenja, Cena = @Cena, IDTipaVozila = @IDTipaVozila
    WHERE KodPaketa = @StariKod
GO
