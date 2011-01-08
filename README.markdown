# Introduction

cleanCore is an C# interface to the WoW Game

# Security

Apart from module scans, cleanCore is vulnerable to Warden scans by making two modifications:

1. detouring a LUA function to provide access to events
2. detouring EndScene to provide a pulse

the latter is considered safe; if you worry about the first one, deactivate events.

# License

cleanCore is licensed under the FreeBSD license or Simplified BSD license.