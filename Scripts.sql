CREATE TABLE sala (
    id_sala integer NOT NULL PRIMARY KEY AUTOINCREMENT,
    nombre varchar NOT NULL,
    ciudad varchar NOT NULL,
    pais varchar NULL,
    tipo varchar NOT NULL,
    capacidad int NOT NULL, 
    disponible boolean NOT NULL);

CREATE TABLE documento (
    id_documento integer NOT NULL PRIMARY KEY AUTOINCREMENT,
    tipo varchar NOT NULL);
	
CREATE TABLE responsable (
    id_responsable double NOT NULL PRIMARY KEY,
    id_documentoFK integer NOT NULL,
    nombre varchar NOT NULL,
    apellido varchar NULL,
    cargo varchar NOT NULL,
    fecha_nacimiento datetime NULL, 
	FOREIGN KEY(id_documentoFK) REFERENCES documento(id_documento));
	

CREATE TABLE reserva (
    id_reserva integer NOT NULL PRIMARY KEY AUTOINCREMENT,
    id_salaFK integer NOT NULL,
    id_responsableFK double NOT NULL,
    cantidad_personas integer NOT NULL,
    fecha_inicio datetime NOT NULL,
    fecha_fin datetime NOT NULL, 
	catering boolean NOT NULL,
	FOREIGN KEY(id_responsableFK) REFERENCES responsable(id_responsable),
	FOREIGN KEY(id_salaFK) REFERENCES sala(id_sala));
	
CREATE TABLE menu (
    id_menu integer NOT NULL PRIMARY KEY AUTOINCREMENT,
    nombre varchar NOT NULL);
	
CREATE TABLE reserva_menu (
    id_reservaFK int NOT NULL,
    id_salaFK int NOT NULL,
    id_menuFK int NOT NULL,
    cantidad_personas int NOT NULL,
	PRIMARY KEY(id_reservaFK,id_salaFK,id_menuFK)
	FOREIGN KEY(id_reservaFK) REFERENCES reserva(id_reserva),
	FOREIGN KEY(id_salaFK) REFERENCES sala(id_sala),
	FOREIGN KEY(id_menuFK) REFERENCES menu(id_menu));
	