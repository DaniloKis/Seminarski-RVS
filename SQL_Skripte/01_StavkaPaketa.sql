
USE KurirskaSluzba2026;
GO

IF OBJECT_ID('dbo.StavkaPaketa', 'U') IS NOT NULL
    DROP TABLE dbo.StavkaPaketa;
GO

CREATE TABLE dbo.StavkaPaketa
(
    Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    KodPaketa     NVARCHAR(13)      NOT NULL,
    NazivArtikla  NVARCHAR(100)     NOT NULL,
    Kolicina      INT               NOT NULL DEFAULT (1),
    TezinaStavke  DECIMAL(10,2)     NOT NULL DEFAULT (0),

    CONSTRAINT FK_StavkaPaketa_Paket
        FOREIGN KEY (KodPaketa)
        REFERENCES dbo.Paket (KodPaketa)
        ON DELETE CASCADE
);
GO

CREATE INDEX IX_StavkaPaketa_KodPaketa ON dbo.StavkaPaketa (KodPaketa);
GO
