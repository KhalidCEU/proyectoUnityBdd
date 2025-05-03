-- Pedidos
-- Buscar
SELECT * FROM "Orders" WHERE "Id" = ?;

-- Insertar
INSERT INTO "Orders" (
  "Id", "Date", "CustomerId", "TotalAmount"
) VALUES (?, ?, ?, ?);

-- Modificar
UPDATE "Orders" SET
  "Date"        = ?,
  "CustomerId"  = ?,
  "TotalAmount" = ?
WHERE "Id" = ?;

-- Eliminar
DELETE FROM "Orders" WHERE "Id" = ?;
