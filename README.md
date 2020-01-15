# DevkitLibrary
PS3, Xbox 360 Devkit

Example:
```
DevKits devKits = new  DevKits();
devkits.SetTarget(DevkitTarget.PS3, 0); // Initialize

ConnectStat state = await devkits.ConnectTargetAsync();
```

## Author
* **Coreizer**

## License
[Public License v3.0](LICENSE)
