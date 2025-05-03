-- Productos
-- Buscar
SELECT * FROM "Products" WHERE "Id" = ?;

-- Insertar
INSERT INTO "Products" (
  "Id", "Name", "CategoryId", "Price", "Stock"
) VALUES (?, ?, ?, ?, ?);

-- Modificar
UPDATE "Products" SET
  "Name"       = ?,
  "CategoryId" = ?,
  "Price"      = ?,
  "Stock"      = ?
WHERE "Id" = ?;

-- Eliminar
DELETE FROM "Products" WHERE "Id" = ?;

