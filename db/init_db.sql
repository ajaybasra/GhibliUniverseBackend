CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Films" (
    "Id" uuid NOT NULL,
    "Title_Value" text NULL,
    "Description_Value" text NULL,
    "Director_Value" text NULL,
    "Composer_Value" text NULL,
    "ReleaseYear_Value" integer NOT NULL,
    CONSTRAINT "PK_Films" PRIMARY KEY ("Id")
);

CREATE TABLE "VoiceActors" (
    "Id" uuid NOT NULL,
    "Name_Value" text NULL,
    CONSTRAINT "PK_VoiceActors" PRIMARY KEY ("Id")
);

CREATE TABLE "Reviews" (
    "Id" uuid NOT NULL,
    "Rating_Value" integer NOT NULL,
    "FilmId" uuid NOT NULL,
    CONSTRAINT "PK_Reviews" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Reviews_Films_FilmId" FOREIGN KEY ("FilmId") REFERENCES "Films" ("Id") ON DELETE CASCADE
);

CREATE TABLE "FilmVoiceActor" (
    "FilmsId" uuid NOT NULL,
    "VoiceActorsId" uuid NOT NULL,
    CONSTRAINT "PK_FilmVoiceActor" PRIMARY KEY ("FilmsId", "VoiceActorsId"),
    CONSTRAINT "FK_FilmVoiceActor_Films_FilmsId" FOREIGN KEY ("FilmsId") REFERENCES "Films" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_FilmVoiceActor_VoiceActors_VoiceActorsId" FOREIGN KEY ("VoiceActorsId") REFERENCES "VoiceActors" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_FilmVoiceActor_VoiceActorsId" ON "FilmVoiceActor" ("VoiceActorsId");

CREATE INDEX "IX_Reviews_FilmId" ON "Reviews" ("FilmId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230615213710_InitialCreate', '7.0.5');

INSERT INTO "Films" ("Id", "Title_Value", "Description_Value", "Director_Value", "Composer_Value", "ReleaseYear_Value")
VALUES ('00000000-0000-0000-0000-000000000001', 'Spirited Away', 'A young girl, Chihiro, finds herself trapped in a spirit world after her parents are transformed into pigs by a witch. In order to save her parents and find a way back home, she must work in a bathhouse for spirits and navigate through various challenges and encounters.', 'Hayao Miyazaki', 'Joe Hisaishi', 2001);

INSERT INTO "VoiceActors" ("Id", "Name_Value")
VALUES ('00000000-0000-0000-0000-000000000001', 'Rumi Hiiragi'),
       ('00000000-0000-0000-0000-000000000002', 'Miyu Irino'),
       ('00000000-0000-0000-0000-000000000003', 'Mari Natsuki'),
       ('00000000-0000-0000-0000-000000000004', 'Bunta Sugawara');

INSERT INTO "Reviews" ("Id", "Rating_Value", "FilmId")
VALUES ('00000000-0000-0000-0000-000000000001', 9, '00000000-0000-0000-0000-000000000001'),
       ('00000000-0000-0000-0000-000000000002', 10, '00000000-0000-0000-0000-000000000001');
       
COMMIT;
