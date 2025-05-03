-- Categorías
-- Buscar
SELECT * FROM "Categories" WHERE "Id" = ?;

-- Insertar
INSERT INTO "Categories" (
  "Id", "Name"
) VALUES (?, ?);

-- Modificar
UPDATE "Categories" SET
  "Name" = ?
WHERE "Id" = ?;

-- Eliminar
DELETE FROM "Categories" WHERE "Id" = ?;
