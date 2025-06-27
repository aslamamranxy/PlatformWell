## Appsettings format

```
{
  "ConnectionStrings": {
    "appDbDSN": ""
  },
  "ApiAuth": {
    "Username": "",
    "Password": ""
  }
}
```

## SQL query to get last updated well for each platform

```
WITH LastUpdatedWell AS (
  SELECT
    p.UniqueName AS PlatformName,
    w.Id,
    w.PlatformId,
    w.UniqueName,
    w.Latitude,
    w.Longitude,
    w.CreatedAt,
    w.LastUpdate as UpdatedAt,
    ROW_NUMBER() OVER (PARTITION BY w.PlatformId ORDER BY w.LastUpdate DESC) AS rank
  FROM Platform p
  INNER JOIN Well w ON p.Id = w.PlatformId
)
SELECT
  PlatformName,
  Id,
  PlatformId,
  UniqueName,
  Latitude,
  Longitude,
  CreatedAt,
  UpdatedAt
FROM LastUpdatedWell
WHERE rank = 1;
```
