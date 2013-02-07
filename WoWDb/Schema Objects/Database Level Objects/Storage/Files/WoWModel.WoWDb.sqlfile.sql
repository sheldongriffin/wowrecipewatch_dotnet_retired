ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [WoWModel.WoWDb], FILENAME = '$(DefaultDataPath)$(DatabaseName).mdf', FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];

