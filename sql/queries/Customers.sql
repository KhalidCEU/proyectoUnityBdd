-- Clientes
-- Buscar
SELECT * FROM "Customers" WHERE "Id" = ?;

-- Insertar
INSERT INTO "Customers" (
  "Id", "Name", "Email", "PhoneNumber", "Address"
) VALUES (?, ?, ?, ?, ?);

-- Modificar
UPDATE "Customers" SET
  "Name"        = ?,
  "Email"       = ?,
  "PhoneNumber" = ?,
  "Address"     = ?
WHERE "Id" = ?;

-- Eliminar
DELETE FROM "Customers" WHERE "Id" = ?;