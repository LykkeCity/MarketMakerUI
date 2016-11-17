# MarketMakersUI

A website to manage Market Makers.

# How to create the configuration file?

* Add these settings in the environmental variables on the host:

  *  SettingsConnString - Connection string that points to the azure blob that contains the main settings file.
  *  SettingsContainerName - Name of the blob container where the global settings file is stored.
  *  SettingsFileName - Name of the global settings file ( including extension ).
  *  LogsConnString - Connection string that points to the azure storage that contains logs.

* Upload the global settings file to the blob storage where the SettingsConnString is pointing. The filename should equal SettingsFileName and the Blob container name that it is uploaded to should equal SettingsContainerName.
