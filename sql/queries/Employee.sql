SELECT * FROM "Employees" WHERE "Id" = ?;

-- Insertar
INSERT INTO "Employees" (
  "Id", "Name", "PositionId", "Salary", "Email", "Photo", "StoreId"
) VALUES (?, ?, ?, ?, ?, ?, ?);

-- Modificar
UPDATE "Employees" SET
  "Name"       = ?,
  "PositionId" = ?,
  "Salary"     = ?,
  "Email"      = ?,
  "Photo"      = ?,
  "StoreId"    = ?
WHERE "Id" = ?;

-- Eliminar
DELETE FROM "Employees" WHERE "Id" = ?;