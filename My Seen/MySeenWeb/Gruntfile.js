//// <binding AfterBuild='concat, uglify, purifycss, cssmin'/>
module.exports = function(grunt) {

    var srcScriptFiles = [
        //jquery
        //"Scripts/jquery-*.min.js",
        "Scripts/jquery-2.2.3.js",
        //"node_modules/jquery/dist/jquery.js", //13/06/16 bootstrap.js:15 Uncaught Error: Bootstrap's JavaScript requires jQuery version 1.9.1 or higher, but lower than version 3 
        "node_modules/jquery-validation/dist/jquery.validate.js",
        //angular
        "node_modules/angular/angular.js",
        "node_modules/angular-ui-router/release/angular-ui-router.js",
        "node_modules/angular-animate/angular-animate.js",
        "node_modules/angular-ui-bootstrap/dist/ui-bootstrap-tpls.js",
        //my app
        "Scripts/myseen/jQueryToAngular.js",
        "Content/Angular/App/app.js",
        "Content/Angular/controllers/*.js",
        "Content/Angular/controllers/Admin/*.js",
        "Content/Angular/controllers/MyMemory/*.js",
        "Content/Angular/controllers/MyMemory/Shared/*.js",
        "Content/Angular/controllers/Portal/*.js",
        "Content/Angular/controllers/Tests/*.js",
        //amcharts
        "node_modules/amcharts/dist/amcharts/amcharts.js",
        "node_modules/amcharts/dist/amcharts/serial.js",
        "node_modules/amcharts/dist/amcharts/themes/light.js",
        //tools
        "Scripts/modernizr-*.js",//бле не нашел в ноде норм скрипт, туча исходников...
        //bootstrap
        "node_modules/bootstrap/dist/js/bootstrap.js",
        //"node_modules/respond/main.js",
        //gmap3
        "Scripts/gmap3.js",
        "Scripts/myseen/gmap.js",
        "Scripts/myseen/gmap.tools.js",
        "Scripts/myseen/variables.js",
        "Scripts/myseen/gmap3menu.js",
        "Scripts/myseen/gmap.editor.js",
        //tools
        "node_modules/moment/min/moment-with-locales.js",
        "node_modules/eonasdan-bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js",
        "Scripts/myseen/jsNlog.js",
        "Scripts/myseen/timer.js",
        //test
        "Content/Angular/templates/Tests/skillsdata.js",
        "Content/Angular/templates/Tests/skill.js"
    ];

    var srcCssFiles = [
        "node_modules/eonasdan-bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.css",
        "node_modules/bootstrap/dist/css/bootstrap-theme.css",
        "Content/myseen/Site.css",
        "Content/myseen/navbar.css",
        "Content/font-awesome.css",
        "Content/myseen/skill.css",
        "Content/myseen/gmap3-menu.css"
    ];

    var srcHtmlFiles = [
        "Content/Angular/templates/*.html",
        "Content/Angular/templates/_tools/*.html",
        "Content/Angular/templates/Admin/*.html",
        "Content/Angular/templates/MyMemory/*.html",
        "Content/Angular/templates/MyMemory/Shared/*.html",
        "Content/Angular/templates/Portal/*.html",
        "Content/Angular/templates/Tests/*.html",

        "Views/Account/*.cshtml",
        "Views/Ads/*.cshtml",
        "Views/Analytics/*.cshtml",
        "Views/Json/*.cshtml",
        "Views/Json/Parts/*.cshtml",
        "Views/Tools/*.cshtml",
        "Views/Tools/Services/Google/*.cshtml"
    ];

    grunt.initConfig({
        pkg: grunt.file.readJSON("package.json"),

        concat: {
            js: {
                src: srcScriptFiles,
                dest: "Content/prod/production.js"
            },
            css:
            {
                src: srcCssFiles,
                dest: "Content/prod/production.css"
            }
        },
        uglify: {
            js: {
                src: "Content/prod/production.js",
                dest: "Content/prod/production.min.js",
                options: {
                    mangle: false
                }
            }
        },
        purifycss: {
            options: {},
            target: {
                src: srcHtmlFiles,
                css: ["Content/prod/production.css"],
                dest: "Content/prod/production.purify.css"
            }
        },
        cssmin: {
            css: {
                src: "Content/prod/production.purify.css",
                dest: "Content/prod/production.min.css"
            }
        }
        ,
        watch: {
            scripts: {
                files: srcScriptFiles,
                tasks: ["concat:js"],
                options: {
                    livereload: true,
                    spawn: false
                }
            },
            css: {
                files: srcCssFiles,
                tasks: ["concat:css"],
                options: {
                    livereload: true,
                    spawn: false
                }
            }
        }
    });

    grunt.loadNpmTasks("grunt-contrib-concat");
    grunt.loadNpmTasks("grunt-contrib-uglify");
    grunt.loadNpmTasks("grunt-contrib-cssmin");
    grunt.loadNpmTasks("grunt-devtools");
    grunt.loadNpmTasks("grunt-purifycss");
    grunt.loadNpmTasks("grunt-contrib-watch");

    grunt.registerTask("default", ["concat", "uglify", "purifycss", "cssmin"]);
};