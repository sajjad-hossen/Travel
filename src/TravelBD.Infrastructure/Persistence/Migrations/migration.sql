CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "Locations" (
    "Id" uuid NOT NULL,
    "Name" character varying(120) NOT NULL,
    "BanglaName" text,
    "Slug" character varying(140) NOT NULL,
    "Type" integer NOT NULL,
    "District" text NOT NULL,
    "Division" text NOT NULL,
    "Latitude" double precision,
    "Longitude" double precision,
    "IsMajorHub" boolean NOT NULL,
    "IsTouristDestination" boolean NOT NULL,
    "Description" text,
    "HeroImageUrl" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Locations" PRIMARY KEY ("Id")
);

CREATE TABLE "Accommodations" (
    "Id" uuid NOT NULL,
    "LocationId" uuid NOT NULL,
    "Name" text NOT NULL,
    "BudgetLevel" integer NOT NULL,
    "ApproxPriceRange" text NOT NULL,
    "Address" text,
    "ContactPhone" text,
    "BookingUrl" text,
    "Rating" double precision NOT NULL,
    "HighlightFeature" text,
    CONSTRAINT "PK_Accommodations" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Accommodations_Locations_LocationId" FOREIGN KEY ("LocationId") REFERENCES "Locations" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Attractions" (
    "Id" uuid NOT NULL,
    "LocationId" uuid NOT NULL,
    "Name" text NOT NULL,
    "BanglaName" text,
    "Description" text,
    "BestTimeToVisit" text,
    "EntryFeeBdt" numeric NOT NULL,
    "ImageUrl" text,
    "Category" text,
    CONSTRAINT "PK_Attractions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Attractions_Locations_LocationId" FOREIGN KEY ("LocationId") REFERENCES "Locations" ("Id") ON DELETE CASCADE
);

CREATE TABLE "DestinationAdvisories" (
    "Id" uuid NOT NULL,
    "LocationId" uuid NOT NULL,
    "Category" text NOT NULL,
    "Title" text NOT NULL,
    "Content" text NOT NULL,
    "IsMandatory" boolean NOT NULL,
    CONSTRAINT "PK_DestinationAdvisories" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_DestinationAdvisories_Locations_LocationId" FOREIGN KEY ("LocationId") REFERENCES "Locations" ("Id") ON DELETE CASCADE
);

CREATE TABLE "RouteSegments" (
    "Id" uuid NOT NULL,
    "OriginId" uuid NOT NULL,
    "DestinationId" uuid NOT NULL,
    "DistanceKm" double precision NOT NULL,
    "AvgDurationMinutes" integer NOT NULL,
    "IsActive" boolean NOT NULL,
    CONSTRAINT "PK_RouteSegments" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RouteSegments_Locations_DestinationId" FOREIGN KEY ("DestinationId") REFERENCES "Locations" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RouteSegments_Locations_OriginId" FOREIGN KEY ("OriginId") REFERENCES "Locations" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "TransportOptions" (
    "Id" uuid NOT NULL,
    "RouteSegmentId" uuid NOT NULL,
    "Mode" integer NOT NULL,
    "Tier" integer NOT NULL,
    "OperatorName" text NOT NULL,
    "MinCostBdt" numeric NOT NULL,
    "MaxCostBdt" numeric NOT NULL,
    "FrequencyPerDay" integer NOT NULL,
    "DepartureStation" text,
    "ArrivalStation" text,
    "BookingLinksJson" text,
    "ScheduleNotes" text,
    CONSTRAINT "PK_TransportOptions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_TransportOptions_RouteSegments_RouteSegmentId" FOREIGN KEY ("RouteSegmentId") REFERENCES "RouteSegments" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Accommodations_LocationId" ON "Accommodations" ("LocationId");

CREATE INDEX "IX_Attractions_LocationId" ON "Attractions" ("LocationId");

CREATE INDEX "IX_DestinationAdvisories_LocationId" ON "DestinationAdvisories" ("LocationId");

CREATE UNIQUE INDEX "IX_Locations_Slug" ON "Locations" ("Slug");

CREATE INDEX "IX_RouteSegments_DestinationId" ON "RouteSegments" ("DestinationId");

CREATE UNIQUE INDEX "IX_RouteSegments_OriginId_DestinationId" ON "RouteSegments" ("OriginId", "DestinationId");

CREATE INDEX "IX_TransportOptions_RouteSegmentId" ON "TransportOptions" ("RouteSegmentId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20261005171258_InitialPostgresMigration', '9.0.2');

COMMIT;

