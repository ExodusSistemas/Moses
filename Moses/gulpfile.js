/// <binding AfterBuild='Pack' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/
var gulp = require('gulp');
var shell = require('gulp-shell');
var filecopy = require('filecopy');
var copyDir = require('copy-dir');
var env = require('gulp-env');
var project = require('./project.json');
var settings = require('./Properties/launchSettings.json');
var apiKey = 'f40c6510-a483-42f8-af9c-7977b6e98637';

var configuration = 'Release';
var profile = 'Release';
var nugetPath = 'e:/SoftPath/';
var packingKey = '4003d786-cc37-4004-bfdf-c4f3e8ef9b3a';

var config = {
   
    GetWorkingDirectory : function(){
        return settings.profiles[configuration].workingDirectory + project.description + "/";
    },

    GetOutputPath : function(){
        return settings.profiles[configuration].workingDirectory + settings.profiles[configuration].environmentVariables.outputPath + settings.profiles[configuration].sdkVersion;
    },

    GetPackageTarget: function () {
        return settings.profiles[configuration].workingDirectory + settings.profiles[configuration].environmentVariables.packageTarget;
    },

    GetSdkVersion: function () {
        return settings.profiles[configuration].sdkVersion;
    }

}

gulp.task("SignAssembly", function () {
    return gulp.src("*.js", { read: false }).pipe(
        shell(["C:/WINDOWS/Microsoft.NET/Framework/v2.0.50727/ilasm.exe Resources/Trirand.Web.Mvc/Trirand.Web.Mvc.il /dll /key=Resources/Trirand.Web.Mvc/key.snk /Out=config/lib/Trirand.Web.Mvc.dll"])
    )
})

gulp.task('GetVars', function () {
    console.dir(config.GetWorkingDirectory());
});

gulp.task('Clean', function () {
    return console.info('Step 0 - Clean');
});

gulp.task('Build', 
   shell.task(['dnu build --configuration=' + configuration + ' --framework=net46 --out=' + config.GetWorkingDirectory()])
);

gulp.task('Prepare', ['Build'], function () {
    console.info('Preparing file copy...');
    //Get Dll directly from output path and copy them to nuget lib structure
    filecopy([config.GetOutputPath() + '/*.dll', config.GetOutputPath() + '/*.xml'], config.GetPackageTarget() + '/lib/' + config.GetSdkVersion());
    return copyDir.sync('config', config.GetPackageTarget());
});

gulp.task('Prepare-Only', function () {
    console.info('Preparing file copy...');
    filecopy([config.GetOutputPath() + '/*.dll', config.GetOutputPath() + '/*.xml'], config.GetPackageTarget() + '/lib/' + config.GetSdkVersion());
    return copyDir.sync('config', config.GetPackageTarget());
});

gulp.task('Pack', ['Prepare'] , function() {
    console.info('Packing nuspec...' + project.version);
    var t = shell.task([nugetPath + 'nuget pack ' + config.GetPackageTarget() + '/' + project.description + '.nuspec -Version ' + project.version + ' -OutputDirectory ' + config.GetPackageTarget() + '/../']);
    t();
    return console.info('Done!');
});

gulp.task('Pack-Only', ['Prepare-Only'], function () {
    console.info('Packing nuspec...');
    var t = shell.task([nugetPath + 'nuget pack ' + config.GetPackageTarget() + '/' + project.description + '.nuspec -Version ' + project.version + ' -OutputDirectory ' + config.GetPackageTarget() + '/../']);
    t();
    return console.info('Done!');
});

gulp.task('Deploy', ['Pack'] , 
   function () {
       return console.info('Deploying Package...');
   }
);

gulp.task('Publish', ['Deploy'], function () {
    var t = shell.task([nugetPath + 'nuget push ' + config.GetPackageTarget() + '../' + project.description + "." + project.version + '.nupkg ' + apiKey]);
    return t();
});

gulp.task('CleanPublish', ['Clean', 'Deploy'], function () {

});
