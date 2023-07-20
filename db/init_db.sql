CREATE TABLE "Films" (
    "Id" UUID,
    "Title" TEXT,
    "Description" TEXT,
    "Director" TEXT,
    "Composer" TEXT,
    "ReleaseYear" INT,
    PRIMARY KEY ("Id")
);

CREATE TABLE "VoiceActors" (
    "Id" UUID,
    "Name" TEXT,
    PRIMARY KEY ("Id")
);

CREATE TABLE "Reviews" (
    "Id" UUID,
    "Rating" INTEGER,
    "FilmId" UUID,
    PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Reviews_Films_FilmId" FOREIGN KEY ("FilmId") REFERENCES "Films" ("Id") ON DELETE CASCADE
);

CREATE TABLE "FilmVoiceActor" (
    "FilmsId" UUID,
    "VoiceActorsId" UUID,
    CONSTRAINT "PK_FilmVoiceActor" PRIMARY KEY ("FilmsId", "VoiceActorsId"),
    CONSTRAINT "FK_FilmVoiceActor_Films_FilmsId" FOREIGN KEY ("FilmsId") REFERENCES "Films" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_FilmVoiceActor_VoiceActors_VoiceActorsId" FOREIGN KEY ("VoiceActorsId") REFERENCES "VoiceActors" ("Id") ON DELETE CASCADE
);

INSERT INTO "Films"("Id", "Title", "Description", "Director", "Composer", "ReleaseYear")
VALUES ('00000000-0000-0000-0000-000000000001', 'Spirited Away', 'A young girl, Chihiro, finds herself trapped in a spirit world after her parents are transformed into pigs by a witch. In order to save her parents and find a way back home, she must work in a bathhouse for spirits and navigate through various challenges and encounters.', 'Hayao Miyazaki', 'Joe Hisaishi', 2001) ON CONFLICT DO NOTHING;

INSERT INTO "VoiceActors"("Id", "Name")
VALUES ('00000000-0000-0000-0000-000000000001', 'Rumi Hiiragi'),
('00000000-0000-0000-0000-000000000002', 'Miyu Irino'),
('00000000-0000-0000-0000-000000000003', 'Mari Natsuki'),
('00000000-0000-0000-0000-000000000004', 'Bunta Sugawara') ON CONFLICT DO NOTHING;

INSERT INTO "Reviews"("Id", "Rating", "FilmId")
VALUES ('00000000-0000-0000-0000-000000000001', 9, '00000000-0000-0000-0000-000000000001'),
('00000000-0000-0000-0000-000000000002', 10, '00000000-0000-0000-0000-000000000001') ON CONFLICT DO NOTHING;