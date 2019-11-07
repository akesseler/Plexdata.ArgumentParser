<p align="center">
  <a href="https://github.com/akesseler/Plexdata.ArgumentParser/blob/master/LICENSE.md" alt="license">
    <img src="https://img.shields.io/github/license/akesseler/Plexdata.ArgumentParser.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.ArgumentParser/releases/latest" alt="latest">
    <img src="https://img.shields.io/github/release/akesseler/Plexdata.ArgumentParser.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.ArgumentParser/archive/master.zip" alt="master">
    <img src="https://img.shields.io/github/languages/code-size/akesseler/Plexdata.ArgumentParser.svg" />
  </a>
  <a href="https://akesseler.github.io/Plexdata.ArgumentParser" alt="docs">
    <img src="https://img.shields.io/badge/docs-guide-orange.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.ArgumentParser/wiki" alt="wiki">
    <img src="https://img.shields.io/badge/wiki-API-orange.svg" />
  </a>
</p>

## Plexdata Argument Parser

The _Plexdata Argument Parser_ is a library that allows users to easily parse the command line arguments given to a program. The main feature of this library is that users only need to define their own class representing all possible command line arguments. Thereafter, each of the properties is tagged by an attribute which describes the kind of the expected command line argument.

At runtime an instance of this pre-defined class is consigned to the _Plexdata Argument Parser_ together with the actual command line arguments. After the parsing procedure, the class contains the values that have been assigned by command line.

Another feature of the _Plexdata Argument Parser_ is the possibility of an automated generation of the help text. For this purpose the pre-defined class is tagged by a suitable set of attributes that include all information needed to create the help text.

For an introduction see the Docs under [https://akesseler.github.io/Plexdata.ArgumentParser/](https://akesseler.github.io/Plexdata.ArgumentParser/).
