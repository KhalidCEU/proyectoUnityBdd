-- Tiendas
-- Buscar
SELECT * FROM "Stores" WHERE "Id" = ?;

-- Insertar
INSERT INTO "Stores" (
  "Id", "Address", "ManagerId"
) VALUES (?, ?, ?);

-- Modificar
UPDATE "Stores" SET
  "Address"   = ?,
  "ManagerId" = ?
WHERE "Id" = ?;

-- Eliminar
DELETE FROM "Stores" WHERE "Id" = ?;

