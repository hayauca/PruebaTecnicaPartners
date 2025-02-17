USE [PARTNERS]
GO
/****** Object:  Table [dbo].[Personas]    Script Date: 26/1/2025 14:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Personas](
	[Identificador] [int] IDENTITY(1,1) NOT NULL,
	[Nombres] [nvarchar](100) NOT NULL,
	[Apellidos] [nvarchar](100) NOT NULL,
	[NumeroIdentificacion] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[TipoIdentificacion] [nvarchar](50) NOT NULL,
	[FechaCreacion] [datetime] NULL,
	[NumeroIdentificacionCompleto]  AS (concat([TipoIdentificacion],'-',[NumeroIdentificacion])) PERSISTED NOT NULL,
	[NombreCompleto]  AS (concat([Nombres],' ',[Apellidos])) PERSISTED NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Identificador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 26/1/2025 14:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Identificador] [int] NOT NULL,
	[Usuario] [nvarchar](50) NOT NULL,
	[Pass] [nvarchar](100) NOT NULL,
	[FechaCreacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Identificador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Personas] ADD  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_Personas] FOREIGN KEY([Identificador])
REFERENCES [dbo].[Personas] ([Identificador])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_Personas]
GO
/****** Object:  StoredProcedure [dbo].[SP_BuscarUsuario]    Script Date: 26/1/2025 14:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_BuscarUsuario]
    @Usuario nvarchar(50)
   
AS
BEGIN
    BEGIN TRY
        SET NOCOUNT ON;

     
        IF EXISTS (SELECT 1 FROM Usuarios WHERE Usuario = @Usuario)
        BEGIN
           
       
            SELECT 'Usuario Existe' AS Mensaje;
        END
        ELSE
        BEGIN
          
            SELECT 'No se encontró el registro con el Identificador proporcionado.' AS Mensaje;
        END
    END TRY
    BEGIN CATCH
      
        SELECT 
            ERROR_NUMBER() AS ErrorNumero,
            ERROR_MESSAGE() AS ErrorMensaje;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[SP_ConsultarPersonas]    Script Date: 26/1/2025 14:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ConsultarPersonas]
AS
BEGIN
    SELECT Identificador, Nombres, Apellidos, NumeroIdentificacion, Email, TipoIdentificacion, FechaCreacion
    FROM Personas;
END;

GO
/****** Object:  StoredProcedure [dbo].[SP_ConsultarUsuarios]    Script Date: 26/1/2025 14:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ConsultarUsuarios]
AS
BEGIN
    SELECT Identificador, Usuario, Pass, FechaCreacion
    FROM Usuarios;
END;
GO
/****** Object:  StoredProcedure [dbo].[SP_EditarPersona]    Script Date: 26/1/2025 14:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_EditarPersona]
    @Identificador INT,
    @Nombres NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @NumeroIdentificacion NVARCHAR(50),
    @Email NVARCHAR(100),
    @TipoIdentificacion NVARCHAR(50)
AS
BEGIN
    BEGIN TRY
        SET NOCOUNT ON;

    
        IF EXISTS (SELECT 1 FROM Personas WHERE Identificador = @Identificador)
        BEGIN
          
            UPDATE Personas
            SET 
                Nombres = @Nombres,
                Apellidos = @Apellidos,
                NumeroIdentificacion = @NumeroIdentificacion,
                Email = @Email,
                TipoIdentificacion = @TipoIdentificacion
            WHERE Identificador = @Identificador;

       
            SELECT 'Registro actualizado correctamente.' AS Mensaje;
        END
        ELSE
        BEGIN
          
            SELECT 'No se encontró el registro con el Identificador proporcionado.' AS Mensaje;
        END
    END TRY
    BEGIN CATCH
      
        SELECT 
            ERROR_NUMBER() AS ErrorNumero,
            ERROR_MESSAGE() AS ErrorMensaje;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[SP_EditarUsuario]    Script Date: 26/1/2025 14:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_EditarUsuario]
    @Identificador INT,
    @Usuario NVARCHAR(50),
    @Pass NVARCHAR(100)
   
AS
BEGIN
    BEGIN TRY
        SET NOCOUNT ON;

        IF EXISTS (SELECT 1 FROM Usuarios WHERE Identificador = @Identificador)
        BEGIN
          
            UPDATE Usuarios
            SET 
                Usuario = @Usuario,
                Pass = @Pass
               
            WHERE Identificador = @Identificador;

       
            SELECT 'Registro actualizado correctamente.' AS Mensaje;
        END
        ELSE
        BEGIN
        
            SELECT 'No se encontró el registro con el Identificador proporcionado.' AS Mensaje;
        END
    END TRY
    BEGIN CATCH
        -- Captura y muestra errores
        SELECT 
            ERROR_NUMBER() AS ErrorNumero,
            ERROR_MESSAGE() AS ErrorMensaje;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[SP_EliminarPersona]    Script Date: 26/1/2025 14:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_EliminarPersona]
    @Identificador INT
   
   
AS
BEGIN
    BEGIN TRY
        SET NOCOUNT ON;

  
        IF EXISTS (SELECT 1 FROM Personas WHERE Identificador = @Identificador)
        BEGIN
           
				IF EXISTS (SELECT 1 FROM Usuarios WHERE Identificador = @Identificador)
			      BEGIN
          	   
				     delete from Usuarios where Identificador=@Identificador;
			
			     END
		   
		   delete from Personas where Identificador=@Identificador;
         
            SELECT 'Registro eliminado.' AS Mensaje;

        END
        ELSE
        BEGIN
         
            SELECT 'No se encontró el registro con el Identificador proporcionado.' AS Mensaje;
        END

    END TRY
    BEGIN CATCH
      
        SELECT 
            ERROR_NUMBER() AS ErrorNumero,
            ERROR_MESSAGE() AS ErrorMensaje;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[SP_EliminarUsuario]    Script Date: 26/1/2025 14:49:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_EliminarUsuario]
    @Identificador INT
   
   
AS
BEGIN
    BEGIN TRY
        SET NOCOUNT ON;

       
        IF EXISTS (SELECT 1 FROM Usuarios WHERE Identificador = @Identificador)
        BEGIN
           			
		   delete from Usuarios where Identificador=@Identificador;
         
            SELECT 'Registro eliminado.' AS Mensaje;

        END
        ELSE
        BEGIN
          
            SELECT 'No se encontró el registro con el Identificador proporcionado.' AS Mensaje;
        END

    END TRY
    BEGIN CATCH

        SELECT 
            ERROR_NUMBER() AS ErrorNumero,
            ERROR_MESSAGE() AS ErrorMensaje;
    END CATCH
END;
GO
