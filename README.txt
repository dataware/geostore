Geostore is a WCF service which allows users to store geo location information in Microsoft SQL Server database.

How to upload data
Geostore service provides a webservice interface to upload compressed json file to server.

In order to upload file client makes a post request with url: 
http://<url>/GeoStoreService.svc/UploadFile

File contents are provided as stream: (stream fileContents)



