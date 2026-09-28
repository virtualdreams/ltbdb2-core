import { minify } from '@node-minify/core';
import { terser } from '@node-minify/terser';
import less from 'less';

const lessBuilder = async ({ settings, content }) => {
	var result = (await less.render(content, settings.options)).css

	return { code: result };
};

minify({
	compressor: lessBuilder,
	input: 'wwwroot/assets/css/*.less',
	output: 'wwwroot/css/ltbdb.min.css',
	options: {
		compress: true
	},
}).then((min) => {
	console.log("Minify LESS => ltbdb.min.css.");
});


minify({
	compressor: terser,
	input: 'wwwroot/assets/js/*.js',
	output: 'wwwroot/js/ltbdb.min.js'
}).then((min) => {
	console.log("Minify JS => ltbdb.min.js.");
});
