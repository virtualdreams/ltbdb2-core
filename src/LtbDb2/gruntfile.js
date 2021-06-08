module.exports = function (grunt) {
	// srcDir:       'wwwroot/assets/'
	// cssTargetDir: 'wwwroot/css/'
	// jsTargetDir:  'wwwroot/js/'

	grunt.initConfig({
		less: {
			target: {
				options: {
					paths: ["wwwroot/assets/"]
				},
				files: {
					"wwwroot/css/material.grunt.css": "wwwroot/assets/css/material.css",
					"wwwroot/css/navigation.grunt.css": "wwwroot/assets/css/navigation.css",
					"wwwroot/css/ltbdb.grunt.css": "wwwroot/assets/css/layout.less",
					"wwwroot/css/admin.grunt.css": "wwwroot/assets/css/admin.less"
				}
			}
		},
		cssmin: {
			target: {
				files: {
					"wwwroot/css/ltbdb.min.css": [
						"wwwroot/css/material.grunt.css",
						"wwwroot/css/ltbdb.grunt.css",
						"wwwroot/css/navigation.grunt.css"
					],
					"wwwroot/css/admin.min.css": [
						"wwwroot/css/material.grunt.css",
						"wwwroot/css/admin.grunt.css",
					]
				}
			}
		},
		uglify: {
			target: {
				files: {
					"wwwroot/js/ltbdb.min.js": ["wwwroot/assets/js/ltbdb.js"]
				}
			}
		},
		clean: {
			cleanup: ["wwwroot/css/*.grunt.*"]
		}
	});

	grunt.loadNpmTasks("grunt-contrib-less");
	grunt.loadNpmTasks("grunt-contrib-cssmin");
	grunt.loadNpmTasks("grunt-contrib-uglify");
	grunt.loadNpmTasks("grunt-contrib-clean");
	grunt.registerTask("default", [
		"less",
		"cssmin",
		"uglify",
		"clean"
	]);
};