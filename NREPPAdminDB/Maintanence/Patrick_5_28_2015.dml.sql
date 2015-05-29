UPDATE RoutingTable
SET DestDescription = 'Return to Pre-Screening'
WHERE CurrentStatus = 3
AND DestStatus = 2

GO