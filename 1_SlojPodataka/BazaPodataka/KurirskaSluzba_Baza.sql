
USE [KurirskaSluzba2026]
GO


IF OBJECT_ID('dbo.Paket', 'U') IS NOT NULL DROP TABLE dbo.Paket
GO
IF OBJECT_ID('dbo.TipVozila', 'U') IS NOT NULL DROP TABLE dbo.TipVozila
GO

CREATE TABLE [dbo].[TipVozila](
    [Sifra] [nvarchar](3)  NOT NULL,
    [Naziv] [nvarchar](40) NOT NULL,
    CONSTRAINT [PK_TipVozila] PRIMARY KEY CLUSTERED ([Sifra] ASC)
)
GO


CREATE TABLE [dbo].[Paket](
    [KodPaketa]             [nvarchar](13) NOT NULL,
    [PosiljalacIme]         [nvarchar](60) NOT NULL,
    [PosiljalacAdresa]      [nvarchar](80) NOT NULL,
    [PosiljalacGrad]        [nvarchar](40) NOT NULL,
    [PrimalacIme]           [nvarchar](60) NOT NULL,
    [PrimalacAdresa]        [nvarchar](80) NOT NULL,
    [PrimalacPostanskiBroj] [nvarchar](10) NOT NULL,
    [PrimalacGrad]          [nvarchar](40) NOT NULL,
    [Tezina]                [decimal](10,2) NOT NULL,
    [OznakaUpozorenja]      [nvarchar](40) NULL,
    [Cena]                  [decimal](10,2) NOT NULL,
    [IDTipaVozila]          [nvarchar](3)  NOT NULL,
    CONSTRAINT [PK_Paket] PRIMARY KEY CLUSTERED ([KodPaketa] ASC)
)
GO

ALTER TABLE [dbo].[Paket] ADD CONSTRAINT [FK_Paket_TipVozila]
    FOREIGN KEY([IDTipaVozila]) REFERENCES [dbo].[TipVozila]([Sifra])
    ON UPDATE CASCADE
GO


IF OBJECT_ID('dbo.Korisnik', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Korisnik](
        [ID]            [int] IDENTITY(1,1) NOT NULL,
        [Prezime]       [nvarchar](40) NOT NULL,
        [Ime]           [nvarchar](30) NOT NULL,
        [KorisnickoIme] [nvarchar](20) NOT NULL,
        [Sifra]         [nvarchar](30) NOT NULL,
        [Status]        [nvarchar](10) NOT NULL,
        CONSTRAINT [PK_Korisnik] PRIMARY KEY CLUSTERED ([ID] ASC)
    )
END
GO

IF NOT EXISTS (SELECT 1 FROM Korisnik WHERE KorisnickoIme = 'admin')
    INSERT INTO Korisnik (Prezime, Ime, KorisnickoIme, Sifra, Status)
    VALUES ('Administrator', 'Glavni', 'admin', 'admin', 'Admin')
GO
IF NOT EXISTS (SELECT 1 FROM Korisnik WHERE KorisnickoIme = 'kurir')
    INSERT INTO Korisnik (Prezime, Ime, KorisnickoIme, Sifra, Status)
    VALUES ('Kuriric', 'Marko', 'kurir', 'kurir', 'Kurir')
GO

IF NOT EXISTS (SELECT 1 FROM TipVozila WHERE Sifra = 'MOT')
    INSERT INTO TipVozila (Sifra, Naziv) VALUES ('MOT', 'Motor')
GO
IF NOT EXISTS (SELECT 1 FROM TipVozila WHERE Sifra = 'KOM')
    INSERT INTO TipVozila (Sifra, Naziv) VALUES ('KOM', 'Kombi')
GO
IF NOT EXISTS (SELECT 1 FROM TipVozila WHERE Sifra = 'KAM')
    INSERT INTO TipVozila (Sifra, Naziv) VALUES ('KAM', 'Kamion')
GO
