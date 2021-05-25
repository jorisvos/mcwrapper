CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE TYPE public.jar_kind AS ENUM
    ('Vanilla', 'Bukkit', 'Spigot', 'Paper', 'Forge');

ALTER TYPE public.jar_kind
    OWNER TO mcwrapper;

-- public.jar definition

-- Drop table

-- DROP TABLE public.jar

CREATE TABLE public.jar
(
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    file_name text NOT NULL UNIQUE,
    jar_kind jar_kind NOT NULL,
    minecraft_version text NOT NULL,
    created_at timestamp with time zone NOT NULL DEFAULT now(),
    modified_at timestamp with time zone,
    UNIQUE (jar_kind, minecraft_version),
    PRIMARY KEY (id)
);

ALTER TABLE public.jar
    OWNER to mcwrapper;

-- public.plugin definition

-- Drop table

-- DROP TABLE public.plugin

CREATE TABLE public.plugin
(
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    name text NOT NULL UNIQUE,
    version text NOT NULL,
    file_name text NOT NULL UNIQUE,
    created_at timestamp with time zone NOT NULL DEFAULT now(),
    modified_at timestamp with time zone,
    PRIMARY KEY (id)
);

ALTER TABLE public.plugin
    OWNER to mcwrapper;

-- public.server definition

-- Drop table

-- DROP TABLE public.server

CREATE TABLE public.server
(
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    name text NOT NULL UNIQUE,
    jar_file uuid NOT NULL,
    java_arguments text NOT NULL DEFAULT '-Xmx1G -Xms1G -jar %jar% nogui',
    enable_plugins boolean NOT NULL DEFAULT false,
    created_at timestamp with time zone NOT NULL DEFAULT now(),
    modified_at timestamp with time zone,
    PRIMARY KEY (id),
    CONSTRAINT jar_file_fk FOREIGN KEY (jar_file)
        REFERENCES public.jar (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
);

ALTER TABLE public.server
    OWNER to mcwrapper;

-- public.plugin_server definition

-- Drop table

-- DROP TABLE public.plugin_server

CREATE TABLE public.plugin_server
(
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    plugin_id uuid NOT NULL,
    server_id uuid NOT NULL,
    created_at timestamp with time zone NOT NULL DEFAULT now(),
    modified_at timestamp with time zone,
    PRIMARY KEY (id),
    CONSTRAINT server_id_fk FOREIGN KEY (server_id)
        REFERENCES public.server (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT plugin_id_fk FOREIGN KEY (plugin_id)
        REFERENCES public.plugin (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
);

ALTER TABLE public.plugin_server
    OWNER to mcwrapper;

-- public.mc_wrapper definition

-- Drop table

-- DROP TABLE public.mc_wrapper

CREATE TABLE public.mc_wrapper
(
    key text NOT NULL,
    value text,
    created_at timestamp with time zone NOT NULL DEFAULT now(),
    modified_at timestamp with time zone,
    PRIMARY KEY (key)
);

ALTER TABLE public.mc_wrapper
    OWNER to mcwrapper;