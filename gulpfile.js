var gulp = require("gulp"),
	concat = require("gulp-concat"),
	rename = require("gulp-rename"),
	cssmin = require("gulp-cssmin"),
	less = require("gulp-less"),
	uglify = require("gulp-uglify")

var cssTargetDir = 'wwwroot/css/'
var jsTargetDir = 'wwwroot/js/'

gulp.task('frontend-css', function() {
	return gulp.src([
		'Assets/css/material.css',
		'Assets/css/layout.less',
		'Assets/css/navigation.css'
	])
	.pipe(less())
	.pipe(concat('ltbdb.min.css'))
	.pipe(cssmin())
	.pipe(gulp.dest(cssTargetDir));
});

gulp.task('admin-css', function() {
	return gulp.src([
		'Assets/css/material.css',
		'Assets/css/admin.less',
	])
	.pipe(less())
	.pipe(concat('admin.min.css'))
	.pipe(cssmin())
	.pipe(gulp.dest(cssTargetDir));
});

gulp.task('error-css', function() {
	return gulp.src([
		'Assets/css/error.less',
	])
	.pipe(less())
	.pipe(concat('error.min.css'))
	.pipe(cssmin())
	.pipe(gulp.dest(cssTargetDir));
});

gulp.task('js', function() {
	return gulp.src([
		'Assets/js/ltbdb.js'
	])
	.pipe(uglify())
	.pipe(rename({suffix: '.min'}))
	.pipe(gulp.dest(jsTargetDir))
});

gulp.task('default', [
	'frontend-css',
	'admin-css',
	'error-css',
	'js'
]);