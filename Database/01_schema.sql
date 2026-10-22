--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2
-- Dumped by pg_dump version 16.2

-- Started on 2026-10-22 22:32:56

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

--
-- TOC entry 4887 (class 1262 OID 66601)
-- Name: mobile_operator; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE mobile_operator WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';


ALTER DATABASE mobile_operator OWNER TO postgres;

\connect mobile_operator

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

--
-- TOC entry 4 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: pg_database_owner
--

CREATE SCHEMA public;


ALTER SCHEMA public OWNER TO pg_database_owner;

--
-- TOC entry 4888 (class 0 OID 0)
-- Dependencies: 4
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: pg_database_owner
--

COMMENT ON SCHEMA public IS 'standard public schema';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 223 (class 1259 OID 66649)
-- Name: call_details; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.call_details (
    id integer NOT NULL,
    client_id integer NOT NULL,
    tariff_id integer NOT NULL,
    date_time timestamp without time zone NOT NULL,
    duration_seconds integer NOT NULL,
    destination_number character varying(20) NOT NULL,
    cost numeric(10,2) NOT NULL
);


ALTER TABLE public.call_details OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 66648)
-- Name: call_details_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.call_details_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.call_details_id_seq OWNER TO postgres;

--
-- TOC entry 4889 (class 0 OID 0)
-- Dependencies: 222
-- Name: call_details_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.call_details_id_seq OWNED BY public.call_details.id;


--
-- TOC entry 221 (class 1259 OID 66633)
-- Name: client_services; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.client_services (
    client_id integer NOT NULL,
    service_id integer NOT NULL
);


ALTER TABLE public.client_services OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 66619)
-- Name: clients; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.clients (
    id integer NOT NULL,
    phone_number character varying(20) NOT NULL,
    full_name character varying(200) NOT NULL,
    balance numeric(12,2) DEFAULT 0 NOT NULL,
    tariff_id integer NOT NULL
);


ALTER TABLE public.clients OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 66618)
-- Name: clients_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.clients_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.clients_id_seq OWNER TO postgres;

--
-- TOC entry 4890 (class 0 OID 0)
-- Dependencies: 219
-- Name: clients_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.clients_id_seq OWNED BY public.clients.id;


--
-- TOC entry 225 (class 1259 OID 66666)
-- Name: offers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.offers (
    id integer NOT NULL,
    client_id integer NOT NULL,
    text text NOT NULL,
    created_at timestamp without time zone DEFAULT now() NOT NULL,
    offer_type character varying(20) NOT NULL
);


ALTER TABLE public.offers OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 66665)
-- Name: offers_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.offers_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.offers_id_seq OWNER TO postgres;

--
-- TOC entry 4891 (class 0 OID 0)
-- Dependencies: 224
-- Name: offers_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.offers_id_seq OWNED BY public.offers.id;


--
-- TOC entry 218 (class 1259 OID 66610)
-- Name: services; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.services (
    id integer NOT NULL,
    name character varying(100) NOT NULL,
    price numeric(10,2) NOT NULL,
    description text
);


ALTER TABLE public.services OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 66609)
-- Name: services_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.services_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.services_id_seq OWNER TO postgres;

--
-- TOC entry 4892 (class 0 OID 0)
-- Dependencies: 217
-- Name: services_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.services_id_seq OWNED BY public.services.id;


--
-- TOC entry 216 (class 1259 OID 66603)
-- Name: tariffs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tariffs (
    id integer NOT NULL,
    name character varying(100) NOT NULL,
    monthly_fee numeric(10,2) NOT NULL,
    call_cost_per_minute numeric(10,2) NOT NULL,
    mb_cost numeric(10,2) NOT NULL,
    tariff_type character varying(20) NOT NULL,
    cashback_percent numeric(5,2),
    credit_limit numeric(10,2),
    penalty_percent numeric(5,2)
);


ALTER TABLE public.tariffs OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 66602)
-- Name: tariffs_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tariffs_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tariffs_id_seq OWNER TO postgres;

--
-- TOC entry 4893 (class 0 OID 0)
-- Dependencies: 215
-- Name: tariffs_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tariffs_id_seq OWNED BY public.tariffs.id;


--
-- TOC entry 4716 (class 2604 OID 66652)
-- Name: call_details id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.call_details ALTER COLUMN id SET DEFAULT nextval('public.call_details_id_seq'::regclass);


--
-- TOC entry 4714 (class 2604 OID 66622)
-- Name: clients id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients ALTER COLUMN id SET DEFAULT nextval('public.clients_id_seq'::regclass);


--
-- TOC entry 4717 (class 2604 OID 66669)
-- Name: offers id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.offers ALTER COLUMN id SET DEFAULT nextval('public.offers_id_seq'::regclass);


--
-- TOC entry 4713 (class 2604 OID 66613)
-- Name: services id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.services ALTER COLUMN id SET DEFAULT nextval('public.services_id_seq'::regclass);


--
-- TOC entry 4712 (class 2604 OID 66606)
-- Name: tariffs id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tariffs ALTER COLUMN id SET DEFAULT nextval('public.tariffs_id_seq'::regclass);


--
-- TOC entry 4730 (class 2606 OID 66654)
-- Name: call_details call_details_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.call_details
    ADD CONSTRAINT call_details_pkey PRIMARY KEY (id);


--
-- TOC entry 4728 (class 2606 OID 66637)
-- Name: client_services client_services_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.client_services
    ADD CONSTRAINT client_services_pkey PRIMARY KEY (client_id, service_id);


--
-- TOC entry 4724 (class 2606 OID 66627)
-- Name: clients clients_phone_number_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT clients_phone_number_key UNIQUE (phone_number);


--
-- TOC entry 4726 (class 2606 OID 66625)
-- Name: clients clients_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT clients_pkey PRIMARY KEY (id);


--
-- TOC entry 4732 (class 2606 OID 66674)
-- Name: offers offers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.offers
    ADD CONSTRAINT offers_pkey PRIMARY KEY (id);


--
-- TOC entry 4722 (class 2606 OID 66617)
-- Name: services services_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.services
    ADD CONSTRAINT services_pkey PRIMARY KEY (id);


--
-- TOC entry 4720 (class 2606 OID 66608)
-- Name: tariffs tariffs_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tariffs
    ADD CONSTRAINT tariffs_pkey PRIMARY KEY (id);


--
-- TOC entry 4736 (class 2606 OID 66655)
-- Name: call_details call_details_client_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.call_details
    ADD CONSTRAINT call_details_client_id_fkey FOREIGN KEY (client_id) REFERENCES public.clients(id) ON DELETE CASCADE;


--
-- TOC entry 4737 (class 2606 OID 66660)
-- Name: call_details call_details_tariff_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.call_details
    ADD CONSTRAINT call_details_tariff_id_fkey FOREIGN KEY (tariff_id) REFERENCES public.tariffs(id);


--
-- TOC entry 4734 (class 2606 OID 66638)
-- Name: client_services client_services_client_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.client_services
    ADD CONSTRAINT client_services_client_id_fkey FOREIGN KEY (client_id) REFERENCES public.clients(id) ON DELETE CASCADE;


--
-- TOC entry 4735 (class 2606 OID 66643)
-- Name: client_services client_services_service_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.client_services
    ADD CONSTRAINT client_services_service_id_fkey FOREIGN KEY (service_id) REFERENCES public.services(id);


--
-- TOC entry 4733 (class 2606 OID 66628)
-- Name: clients clients_tariff_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT clients_tariff_id_fkey FOREIGN KEY (tariff_id) REFERENCES public.tariffs(id);


--
-- TOC entry 4738 (class 2606 OID 66675)
-- Name: offers offers_client_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.offers
    ADD CONSTRAINT offers_client_id_fkey FOREIGN KEY (client_id) REFERENCES public.clients(id) ON DELETE CASCADE;


-- Completed on 2026-10-22 22:32:56

--
-- PostgreSQL database dump complete
--

