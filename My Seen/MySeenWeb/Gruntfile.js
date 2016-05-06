/// <binding AfterBuild='concat, uglify, cssmin' ProjectOpened='watch' />
module.exports = function(grunt) {

    var srcScriptFiles = [
        //jquery
        'Scripts/jquery-*.min.js',
        'Scripts/jquery.validate.js',
        'Scripts/jquery.validate.unobtrusive.js',
        //angular
        'Scripts/angular.js',
        'Scripts/angular-ui-router.js',
        'Scripts/angular-animate.js',
        'Scripts/angular-ui/ui-bootstrap-tpls.js',
        //my app
        'Scripts/myseen/jQueryToAngular.js',
        'Content/Angular/App/app.js',
        'Content/Angular/controllers/*.js',
        'Content/Angular/controllers/Admin/*.js',
        'Content/Angular/controllers/MyMemory/*.js',
        'Content/Angular/controllers/MyMemory/Shared/*.js',
        'Content/Angular/controllers/Portal/*.js',
        'Content/Angular/controllers/Tests/*.js',
        //amcharts
        'Content/amcharts/amcharts.js',
        'Content/amcharts/serial.js',
        'Content/amcharts/themes/light.js',
        'Scripts/modernizr-*.js',
        //bootstrap
        'Scripts/bootstrap.js',
        'Scripts/respond.js',
        //gmap3
        'Scripts/gmap3.js',
        //jsnlog
        'Scripts/myseen/gmap.js',
        'Scripts/myseen/gmap.tools.js',
        'Scripts/myseen/variables.js',
        'Scripts/myseen/gmap3menu.js',
        'Scripts/myseen/gmap.editor.js',
        //tools
        'Scripts/moment-with-locales.js',
        'Scripts/bootstrap-datetimepicker.js',
        'Scripts/myseen/jsNlog.js',
        'Scripts/myseen/timer.js',
        //test
        'Content/Angular/templates/Tests/skillsdata.js',
        'Content/Angular/templates/Tests/skill.js'
    ];

    var srcCssFiles = [
        'Content/myseen/Site.css',
        'Content/myseen/navbar.css',
        'Content/bootstrap-datetimepicker.css',
        'Content/bootstrap-theme.css',
        'Content/animate.css',
        'Content/font-awesome.css',
        'Content/myseen/skill.css',
        'Content/myseen/gmap3-menu.css'
    ];

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        concat: {
            js: {
                src: srcScriptFiles,
                dest: 'Content/prod/production.js'
            },
            css:
            {
                src: srcCssFiles,
                dest: 'Content/prod/production.css'
            }
        },
        uglify: {
            js: {
                src: 'Content/prod/production.js',
                dest: 'Content/prod/production.min.js',
                options: {
                    mangle: false
                }
            }
        },
        cssmin: {
            css: {
                src: 'Content/prod/production.css',
                dest: 'Content/prod/production.min.css'
            }
        },
        watch: {
            grunt_config:
            {
                files: 'Gruntfile.js',
                //tasks: ['concat', 'uglify', 'cssmin'],
                tasks: ['concat'],
                options: {
                    livereload: true,
                    spawn: false
                }
            },
            scripts: {
                files: srcScriptFiles,
                //tasks: ['concat:js', 'uglify:js'],
                tasks: ['concat:js'],
                options: {
                    livereload: true,
                    spawn: false
                }
            },
            css: {
                files: srcCssFiles,
                //tasks: ['concat:css','cssmin'],
                tasks: ['concat:css'],
                options: {
                    livereload: true,
                    spawn: false
                }
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-devtools');
    grunt.loadNpmTasks('grunt-contrib-watch');

    grunt.registerTask('default', ['concat', 'uglify', 'cssmin']);
};