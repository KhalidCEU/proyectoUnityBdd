-- Detalle de Pedidos (ProductOrders)
-- Buscar
SELECT * FROM "ProductOrders"
 WHERE "OrderId" = ?
   AND "ProductId" = ?;

-- Insertar
INSERT INTO "ProductOrders" (
  "OrderId", "ProductId", "Quantity", "PriceAtOrder"
) VALUES (?, ?, ?, ?);

-- Modificar
UPDATE "ProductOrders" SET
  "Quantity"     = ?,
  "PriceAtOrder" = ?
WHERE "OrderId" = ?
  AND "ProductId" = ?;

-- Eliminar
DELETE FROM "ProductOrders"
 WHERE "OrderId"   = ?
   AND "ProductId" = ?;