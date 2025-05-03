-- Posiciones
-- Buscar
SELECT * FROM "Positions" WHERE "Id" = ?;

-- Insertar
INSERT INTO "Positions" (
  "Id", "Name"
) VALUES (?, ?);

-- Modificar
UPDATE "Positions" SET
  "Name" = ?
WHERE "Id" = ?;

-- Eliminar
DELETE FROM "Positions" WHERE "Id" = ?;