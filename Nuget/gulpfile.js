/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var fs = require("fs");
var request = require("request");
var nuget = require('gulp-nuget');
var msbuild = require("gulp-msbuild");
var del = require("delete");


var nugetPath = './';
var nugetExePath = './nuget.exe';

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


var options = {
    nuget: './nuget.exe', //./nuget.exe 
    source: 'https://www.nuget.org/',
    apiKey: 'f40c6510-a483-42f8-af9c-7977b6e98637',
    timeout: '300',
    configFile: './NuGet.config',
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
    if (fs.existsSync(nugetExePath)) {
        return done();
    }
    request.get('http://nuget.org/nuget.exe')
        .pipe(fs.createWriteStream(nugetExePath))
        .on('close', done);
});

gulp.task('nuget-pack', function () {
    var stream = nuget.pack(options);
    var projectVersion = GetProjectVersion();
    return gulp.src(nugetPath + '/Package.nuspec')
      .pipe(nuget.pack({ nuget: nugetExePath, version: projectVersion }))
      .pipe(gulp.dest(nugetPath + '/Packages'));
});

gulp.task('nuget-push', function () {
    console.log('Publishing Moses...');
    var finish = false;
    var print = function(data){
        console.log(data);
        finish = true;
    }
    var stream = gulp.src('./Packages/Moses.3.6.0.4.nupkg')
    .pipe(nuget.push(options));

    return stream;    
});

gulp.task('nuget-deploy', ['build', 'nuget-pack', 'nuget-push'] , function(){

});


gulp.task('project-version', function(){
    GetProjectVersion();
})