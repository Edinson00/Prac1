CREATE TABLE [dbo].[APODERADO]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Nombre] NCHAR(50) NULL, 
    [telefono] NCHAR(50) NULL, 
    [Id_alumno] INT NULL, 
    CONSTRAINT [FK_APODERADO_ALUMNO] FOREIGN KEY ([Id_alumno]) REFERENCES [ALUMNO]([Id]) 
)
