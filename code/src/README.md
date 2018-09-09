## Project Build

Best way to build the whole project is to use _Visual Studio 2017 Community_. Thereafter, download the complete sources, open the solution file ``Plexdata.ArgumentParser.NET.sln``, switch to release and rebuild all.

## Help Generation

The help file with the whole API documentation is not yet available.

## Trouble Shooting

Nothing known at the moment.

## Known Issues

### Argument Recognition

Consider class below because it causes the unexpected behavior that ``IsDebug`` will never be ``true``.

```
[ParametersGroup]
class CmdLineArgs
{
    [SwitchParameter(SolidLabel = "default", BriefLabel = "d")]
    public Boolean IsDefault { get; set; }

    [SwitchParameter(SolidLabel = "verbose", BriefLabel = "v")]
    public Boolean IsVerbose { get; set; }

    [SwitchParameter(SolidLabel = "debug")]
    public Boolean IsDebug { get; set; }
}
```

Also consider, users may call the program with the parameters like shown as follows.

```$> program.exe --debug --default --verbose```

Under that conditions the argument ``--debug`` will never be hit because it is processed as argument ``--default``. The reason behind is that the _Argument Parser_ automatically treats the first character of a _Solid Label_ as the _Brief Label_ if the _Brief Label_ is not set explicitly. Further, the _Brief Label_ ``d`` is actually assigned to property ``IsDefault ``.

#### Workaround

Either you reorder the command line argument class like shown here:
```
[ParametersGroup]
class CmdLineArgs
{
    [SwitchParameter(SolidLabel = "debug")]
    public Boolean IsDebug { get; set; }

    [SwitchParameter(SolidLabel = "default", BriefLabel = "d")]
    public Boolean IsDefault { get; set; }

    [SwitchParameter(SolidLabel = "verbose", BriefLabel = "v")]
    public Boolean IsVerbose { get; set; }
}
```
Or you add a distinct _Brief Label_ to the ``IsDebug `` property like shown here:
```
[SwitchParameter(SolidLabel = "debug", BriefLabel ="g")]
public Boolean IsDebug { get; set; }
```
