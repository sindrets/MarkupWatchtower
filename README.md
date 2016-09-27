#Markup Watchtower
Automatically compile SASS, Jade, HAML and Coffe-Script.
![Markup Watchtower logo](https://raw.githubusercontent.com/sindrets/MarkupWatchtower/master/img/MarkupWatchtowerIcon_HalfSize.png)
<br>Markup Watchtower is a tool designed for speeding up your workflow while working with stylesheet and markup languages that require to be compiled before they can be properly interpreted by the web-browser. While many of these languages provide tools to set up file system watchers through the command line, this application gathers all watchers in one place.
![Figure 1](https://raw.githubusercontent.com/sindrets/MarkupWatchtower/master/img/figure1.PNG)

A watcher can be added simply with the click of a button, and once set up and connected to a directory, the information about the watcher is saved. The application will automatically load and start all your watchers upon launching the program. 

#Compiling
Markup Watchtower still uses the command line preprocessors provided by the developers of the languages, and so it is still required that you have these insalled for the automatic compiling to work. The preprocessors for the languages that are available in this application right now (HAML, SASS, Jade, Pug, Coffee-Script, Babel) can be installed through either Ruby (check http://rubyinstaller.org/) or NPM (https://www.npmjs.com/). 
However, once the tools are installed and you have set up some watchers, Markup Watchtower will automatically detect file changes to files of specified type, and compile them to the same directory they are currently in.

![Figure 2](https://raw.githubusercontent.com/sindrets/MarkupWatchtower/master/img/figure2.PNG)

By default, the Watchtower will monitor all subdirectories of specified folder. This can easily be changed by unchecking the check box labeled "Subdirectories".

#Customizability
Do you have a language you wish was implemented? No worries! It is super simple to add: after running the application once, it will generate a file called "markupList.json". This is where the Watchtower reads to determine the available languages. If you wish to add your own, you just need to follow the JSON format, and specify the required information:
![Figure 2](https://raw.githubusercontent.com/sindrets/MarkupWatchtower/master/img/figure3.PNG)

    "SASS" - This is the property key. When adding your own key, set this to whatever you want it to show up as in the application.
    "input" - This is where you specify what file extensions the watcher will filter. You can specify multiple extensions as seen here with the ".scss" and ".sass" extensions.
    "output" - Here you specify what file extension the compiled document will have.
    "command" - This is the command to be run through cmd. The "!input!" and "!output!" keywords are replaced with the path to the file that is to be compiled and the path to the resulting compiled file, respectively. The keywords do NOT have to be included if these paths are unnecessary to the command.

#Information
For information about:
* HAML, visit: http://haml.info/
* SASS, visit: http://sass-lang.com/
* Jade, visit: https://www.npmjs.com/package/jade
* Pug, visit: https://pugjs.org/
* Coffee-Script, visit: http://coffeescript.org/
* Babel, visit: https://babeljs.io/
  
#Legal Information
Markup Watchtower

Copyright (C) 2016  Sindre T. Strøm

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or 
any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
