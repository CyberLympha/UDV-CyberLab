--
-- PostgreSQL database dump
--

-- Dumped from database version 15.7 (Debian 15.7-1.pgdg120+1)
-- Dumped by pg_dump version 16.1

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Credentials; Type: TABLE; Schema: public; Owner: dev
--

CREATE TABLE public."Credentials" (
    "Id" uuid NOT NULL,
    "VmId" uuid NOT NULL,
    "Username" text NOT NULL,
    "Password" text NOT NULL,
    "Ip" text NOT NULL
);


ALTER TABLE public."Credentials" OWNER TO dev;

--
-- Name: Labs; Type: TABLE; Schema: public; Owner: dev
--

CREATE TABLE public."Labs" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Goal" text NOT NULL,
    "Manual" text NOT NULL,
    "CreatedBy" uuid NOT NULL
);


ALTER TABLE public."Labs" OWNER TO dev;

--
-- Name: Report; Type: TABLE; Schema: public; Owner: dev
--

CREATE TABLE public."Report" (
    "Id" uuid NOT NULL,
    "LastSentAt" timestamp with time zone NOT NULL,
    "Description" text NOT NULL,
    "UserLabId" uuid NOT NULL
);


ALTER TABLE public."Report" OWNER TO dev;

--
-- Name: UserLabStatus; Type: TABLE; Schema: public; Owner: dev
--

CREATE TABLE public."UserLabStatus" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL
);


ALTER TABLE public."UserLabStatus" OWNER TO dev;

--
-- Name: UserLabs; Type: TABLE; Schema: public; Owner: dev
--

CREATE TABLE public."UserLabs" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "LabId" uuid NOT NULL,
    "StatusId" uuid NOT NULL,
    "Rate" integer NOT NULL
);


ALTER TABLE public."UserLabs" OWNER TO dev;

--
-- Name: VirtualMachines; Type: TABLE; Schema: public; Owner: dev
--

CREATE TABLE public."VirtualMachines" (
    "Id" uuid NOT NULL,
    "ProxmoxVmId" integer NOT NULL,
    "UserLabId" uuid NOT NULL,
    "Node" text NOT NULL
);


ALTER TABLE public."VirtualMachines" OWNER TO dev;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: dev
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO dev;

--
-- Data for Name: Credentials; Type: TABLE DATA; Schema: public; Owner: dev
--

COPY public."Credentials" ("Id", "VmId", "Username", "Password", "Ip") FROM stdin;
\.


--
-- Data for Name: Labs; Type: TABLE DATA; Schema: public; Owner: dev
--

COPY public."Labs" ("Id", "Name", "Goal", "Manual", "CreatedBy") FROM stdin;
4eac2760-292b-497a-a755-90d695517c40	Первая лабораторная	string	string	00000000-0000-0000-0000-000000000000
\.


--
-- Data for Name: Report; Type: TABLE DATA; Schema: public; Owner: dev
--

COPY public."Report" ("Id", "LastSentAt", "Description", "UserLabId") FROM stdin;
\.


--
-- Data for Name: UserLabStatus; Type: TABLE DATA; Schema: public; Owner: dev
--

COPY public."UserLabStatus" ("Id", "Name") FROM stdin;
\.


--
-- Data for Name: UserLabs; Type: TABLE DATA; Schema: public; Owner: dev
--

COPY public."UserLabs" ("Id", "UserId", "LabId", "StatusId", "Rate") FROM stdin;
bd6d47f5-57cc-485f-b971-f3584f1656df	d4009072-3e78-40db-a5f8-bf5fe546c5bc	4eac2760-292b-497a-a755-90d695517c40	52059edd-dcb0-49f8-9f37-86fd36649228	0
\.


--
-- Data for Name: VirtualMachines; Type: TABLE DATA; Schema: public; Owner: dev
--

COPY public."VirtualMachines" ("Id", "ProxmoxVmId", "UserLabId", "Node") FROM stdin;
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: dev
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20240603092427_CreateDb	6.0.28
\.


--
-- Name: Credentials PK_Credentials; Type: CONSTRAINT; Schema: public; Owner: dev
--

ALTER TABLE ONLY public."Credentials"
    ADD CONSTRAINT "PK_Credentials" PRIMARY KEY ("Id");


--
-- Name: Labs PK_Labs; Type: CONSTRAINT; Schema: public; Owner: dev
--

ALTER TABLE ONLY public."Labs"
    ADD CONSTRAINT "PK_Labs" PRIMARY KEY ("Id");


--
-- Name: Report PK_Report; Type: CONSTRAINT; Schema: public; Owner: dev
--

ALTER TABLE ONLY public."Report"
    ADD CONSTRAINT "PK_Report" PRIMARY KEY ("Id");


--
-- Name: UserLabStatus PK_UserLabStatus; Type: CONSTRAINT; Schema: public; Owner: dev
--

ALTER TABLE ONLY public."UserLabStatus"
    ADD CONSTRAINT "PK_UserLabStatus" PRIMARY KEY ("Id");


--
-- Name: UserLabs PK_UserLabs; Type: CONSTRAINT; Schema: public; Owner: dev
--

ALTER TABLE ONLY public."UserLabs"
    ADD CONSTRAINT "PK_UserLabs" PRIMARY KEY ("Id");


--
-- Name: VirtualMachines PK_VirtualMachines; Type: CONSTRAINT; Schema: public; Owner: dev
--

ALTER TABLE ONLY public."VirtualMachines"
    ADD CONSTRAINT "PK_VirtualMachines" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: dev
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_Report_UserLabId; Type: INDEX; Schema: public; Owner: dev
--

CREATE UNIQUE INDEX "IX_Report_UserLabId" ON public."Report" USING btree ("UserLabId");


--
-- Name: IX_UserLabs_LabId; Type: INDEX; Schema: public; Owner: dev
--

CREATE INDEX "IX_UserLabs_LabId" ON public."UserLabs" USING btree ("LabId");


--
-- Name: Report FK_Report_UserLabs_UserLabId; Type: FK CONSTRAINT; Schema: public; Owner: dev
--

ALTER TABLE ONLY public."Report"
    ADD CONSTRAINT "FK_Report_UserLabs_UserLabId" FOREIGN KEY ("UserLabId") REFERENCES public."UserLabs"("Id") ON DELETE CASCADE;


--
-- Name: UserLabs FK_UserLabs_Labs_LabId; Type: FK CONSTRAINT; Schema: public; Owner: dev
--

ALTER TABLE ONLY public."UserLabs"
    ADD CONSTRAINT "FK_UserLabs_Labs_LabId" FOREIGN KEY ("LabId") REFERENCES public."Labs"("Id") ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

