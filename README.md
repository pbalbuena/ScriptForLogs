# ScriptForLogs
Script to download many logs from many repositories getting the commits from certain authors.

## How to use

1. You need to have three files in the same directory. 
- ScriptForLogs.exe
- datos.txt
- repositorios.txt

2. Write in `repositorios.txt` the path of the local repositories you want to download. One repository in each line.

```
C:\Users\pgonzal2\source\repos\Repo1
C:\Users\pgonzal2\source\repos\Repo2
C:\Users\pgonzal2\source\repos\Repo3
C:\Users\pgonzal2\source\repos\Repo4
```

3. Write in `datos.txt` the information of the authors in this format

`<UserMapfre>,<UserCap>,<EmailMapfre>,<EmailCap>,[Optional]<BeginDate>,[Optional]<EndDate>`

Date format must be yyyy-MM-dd

If no dates are selected, it will get only from last month until today.

```
Mapfre1,soynombrecap1,soyemailmapfre1,soyemailcap1,2009-12-2,2018-11-01
Mapfre2,soynombrecap2,soyemailmapfre2,soyemailcap2
Mapfre3,soynombrecap3,soyemailmapfre3,soyemailcap3,2015-12-2,2019-03-01
```

4. Execute `ScriptForLogs.exe`

This script will download all the logs of every repository specified in `repositorios.txt` for every user of `datos.txt`.
Also, `UserMapfre` will be replaced by `UserCap` and `EmailMapfre` will be replaced by `EmailCap`.
The resulting logs will be downloaded to the same repository in html format.
