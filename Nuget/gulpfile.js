/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp-param')(require('gulp'), process.argv);;
var fs = require("fs");
var request = require("request");
var nuget = require('gulp-nuget');
var msbuild = require("gulp-msbuild");
var del = require("delete");

var options = {
    nuget: './nuget.exe', //./nuget.exe 
    source: 'https://www.nuget.org/',
    timeout: '300',
    outputDirectory: './Packages/', //./gulp-nuget/ 
    basePath: './',
    properties: 'configuration=release',
    minClientVersion: '2.5',
    msBuildVersion: '12',
    build: true,
    symbols: false,
    excludeEmptyDirectories: true,
    includeReferencedProjects: true,
    noDefaultExcludes: true,
    tool: true,
};

gulp.task('build', function (done) {
    EnsureFolders();
    del("./lib/*")

    var stream = gulp.src("../Moses.Web/Moses.Web.csproj").pipe(
        msbuild({
            targets: ['Clean', 'Build'],
            properties: { WarningLevel: 1 , Configuration: 'Release'} ,
            errorOnFail : false,
            logCommand : true,
            verbosity: 'normal',
            stdout : true,
            toolsVersion: 14.0
        })
    );

    return stream;
});

gulp.task('nuget-version', function (done) {
    GetProjectVersion();
});

gulp.task('nuget-download', function (done) {
    if (fs.existsSync(options.nuget)) {
        return done();
    }
    var stream = request.get('http://nuget.org/nuget.exe')
        .pipe(fs.createWriteStream(options.nuget))
        .on('close', done);

    return stream;
});

gulp.task('nuget-pack', ['nuget-download'], function () {
    EnsureFolders();
    var stream = nuget.pack(options);
    var projectVersion = GetProjectVersion();
    return gulp.src(options.basePath + '/Package.nuspec')
      .pipe(nuget.pack({ nuget: options.nuget, version: projectVersion }))
      .pipe(gulp.dest(options.basePath + '/Packages'));
});

gulp.task('nuget-push', ['nuget-download'], function (apiKey) {
    console.log('Publishing Moses...');
    var projectVersion = GetProjectVersion();
    var finish = false;
    var print = function(data){
        console.log(data);
        finish = true;
    }

    if (apiKey == null)
        throw Erro("Api Key is null (invalid)");

    options.apiKey = apiKey;
    var stream = gulp.src('./Packages/Moses.'+ projectVersion +'.nupkg')
    .pipe(nuget.push(options));

    return stream;    
});

gulp.task('nuget-deploy', ['nuget-push'] , function(apiKey){
    
});


gulp.task('project-version', function(){
    GetProjectVersion();
})

function GetProjectVersion(){
    var version = null;
    var XmlReader = require('xml-reader');
    var xmlQuery = require('xml-query');
    var reader = XmlReader.create();
    var file = "./Package.nuspec";
    var fsync = fs.readFileSync(file,'utf8' );
    var xml = XmlReader.parseSync(fs.readFileSync(file,'utf8'));
    
    //navigating through xml
    version = xmlQuery(xml).find('version').text();
    console.log('Version found: '  + version);

    if (version == null)
        throw Error('Version not found on Nuspec file');
    return version;
}

function EnsureFolders(){
    if (!fs.existsSync("./tools"))
        fs.mkdirSync("./tools", () => {});
    if (!fs.existsSync("./content"))
        fs.mkdirSync("./content", () => {});
}