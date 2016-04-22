module.exports = function (grunt) {

    var srcScriptFiles = [
        //angular
        'Scripts/angular.min.js',
        'Scripts/angular-ui-router.min.js',
        'Scripts/angular-ui/ui-bootstrap-tpls.min.js',
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
        'Scripts/jquery-*.min.js',
        'Scripts/jquery.validate.min.js',
        'Scripts/jquery.validate.unobtrusive.min.js',
        'Scripts/moment-with-locales.min.js',
        'Scripts/bootstrap-datetimepicker.min.js',
        'Scripts/bootstrap.min.js',
        'Scripts/respond.js',
        'Scripts/gmap3.js',
        'Scripts/myseen/jsNlog.js',
        'Scripts/myseen/timer.js',
        'Scripts/myseen/gmap.js',
        'Scripts/myseen/gmap.tools.js',
        'Scripts/myseen/variables.js',
        'Scripts/myseen/jQueryToAngular.js',
        'Scripts/myseen/gmap3menu.js',
        'Scripts/myseen/gmap.editor.js',
        //test
        'Content/Angular/templates/Tests/skillsdata.js',
        'Content/Angular/templates/Tests/skill.js'
    ];

    var srcCssFiles = [
        'Content/myseen/Site.css',
        'Content/myseen/navbar.css',
        'Content/bootstrap-datetimepicker.min.css',
        'Content/bootstrap-theme.min.css',
        'Content/animate.min.css',
        'Content/font-awesome.css',
        'Content/myseen/skill.css',
        'Content/myseen/gmap3-menu.css'
    ];

    // 1. Вся настройка находится здесь
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        concat: {
            dist: {
                src: srcScriptFiles,
                dest: 'Content/prod/production.js'
            }
        },
        concat_css: {
            options: {
                // Task-specific options go here. 
            },
            all: {
                src: srcCssFiles,
                dest: "Content/prod/production.css"
            }
        },
        uglify: {
            build: {
                src: 'Content/prod/production.js',
                dest: 'Content/prod/production.min.js'
            }
        },
        watch: {
            grunt_config:
            {
                files: ['Gruntfile.js'],
                tasks: ['concat', 'uglify', 'concat_css'],
                options: {
                    livereload: true,
                    spawn: false
                }
            },
            scripts: {
                files: srcScriptFiles,
                tasks: ['concat', 'uglify'],
                options: {
                    livereload: true,
                    spawn: false
                }
            },
            css: {
                files: srcCssFiles,
                tasks: ['concat_css'],
                options: {
                    livereload: true,
                    spawn: false
                }
            }
        }
    });

    // 3. Тут мы указываем Grunt, что хотим использовать этот плагин
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-concat-css');
    grunt.loadNpmTasks('grunt-devtools');
    grunt.loadNpmTasks('grunt-contrib-watch');

    // 4. Указываем, какие задачи выполняются, когда мы вводим «grunt» в терминале
    grunt.registerTask('default', ['concat', 'uglify', 'concat_css', 'watch']);

    //wath for waiting
    //devtools for waiting

};