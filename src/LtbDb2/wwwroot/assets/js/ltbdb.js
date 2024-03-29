function split(val) {
	return val.split(/;\s*/);
}

function extractLast(term) {
	return split(term).pop();
}

String.prototype.format = function () {
	var s = this, i = arguments.length;

	while (i--) {
		s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
	}
	return s;
}

String.prototype.formatEx = function (placeholders) {
	var s = this;
	for (var propertyName in placeholders) {
		s = s.replace(new RegExp('\\{' + propertyName + '\\}', 'gm'), placeholders[propertyName]);
	}
	return s;
};

$(function () {
	/* autocomplete for tags */
	$('#tags').autocomplete({
		source: function (request, response) {
			$.getJSON('/search/tag', {
				term: extractLast(request.term)
			}, response);
		},
		search: function () {
			var term = extractLast(this.value);
			if (term.length < 3) {
				return false;
			}
		},
		focus: function () {
			return false;
		},
		select: function (event, ui) {
			var terms = split(this.value);
			terms.pop();
			terms.push(ui.item.value);
			terms.push("");
			this.value = terms.join("; ");
			return false;
		}
	});

	/* autocomplete for categories */
	$('.ac-category').autocomplete({
		source: '/search/category',
		minLength: 0,
		select: function (event, ui) {
			if (ui.item) {
				$(event.target).val(ui.item.value);
			}
		}
	}).focus(function () {
		$(this).autocomplete("search", $(this).val());
	});

	/* autocomplete for a single tag */
	$('.ac-tag').autocomplete({
		source: '/search/tag',
		minLength: 0,
		select: function (event, ui) {
			if (ui.item) {
				$(event.target).val(ui.item.value);
			}
		}
	}).focus(function () {
		$(this).autocomplete("search", $(this).val());
	});

	/* show cover */
	jbox_image = new jBox('Image', {
		// downloadButton: true
	});

	/* add, delete or insert stories */
	var story_container = $('#story-container');
	var story_template =
		'<div class="input-group mb-3" >\
			<input class="form-control" type="text" name="stories" placeholder="Inhalt" />\
			<button class="btn btn-outline-secondary story-ins" type="button"><i class="fa-solid fa-plus"></i></button>\
			<button class="btn btn-outline-secondary story-rem" type="button"><i class="fa-solid fa-minus"></i></button>\
		</div>'

	$(document).on('click', '#story-add', function (e) {
		$(story_template).appendTo(story_container);
	});

	$(document).on('click', '.story-ins', function (e) {
		var p = $(this).parent();
		$(story_template).insertBefore(p);
	});

	$(document).on('click', '.story-rem', function (e) {
		var p = $(this).parent();
		p.remove();
	});

	/* delete book */
	$('#deleteSubmit').click(function () {
		var method = $('#delete').data('method');
		var url = $('#delete').data('url');
		$.ajax({
			type: method,
			url: url
		}).done(function () {
			location.href = '/';
		}).fail(function () {
			$('#error').html('<div class="alert alert-danger">Löschen des Buches fehlgeschlagen.</div>');
		});
	});

	/* add, delete or restore image */
	function reset(e) {
		var control = e;
		control.replaceWith(control.clone(true));
	}

	$('#image').on('click', function () {
		$('#image-upload').click();
	});

	$('#image-upload').change(function () {
		if (typeof window.FileReader === 'undefined') {
			alert("Dein Browser unterstützt diese Funktion nicht. Die Vorschau ist nicht verfügbar.");
			return;
		} else {
			var files = $("#image-upload").get(0).files;
			var reader = new FileReader();

			if (!files[0].type.match('image')) {
				reset($('#image-upload'));
				alert("Dateityp nicht unterstützt.");
				return;
			}

			reader.onload = function (e) {
				$('#image').attr('src', e.target.result);
			}

			reader.readAsDataURL(files[0]);
		}

		$('#remove').val(false);
		$('#image-reset').show();
	});

	$('#image-delete').click(function (e) {
		e.preventDefault();

		reset($('#image-upload'));
		$('#image').attr('src', $(this).data('src'));
		$('#remove').val(true);
		$('#image-reset').show();
	});

	$('#image-reset').click(function (e) {
		e.preventDefault();

		reset($('#image-upload'));
		$('#image').attr('src', $(this).data('src'));
		$('#remove').val(false);
		$('#image-reset').hide();
	});

	/* validation */
	$.validator.setDefaults({
		highlight: function (element) {
			$(element).addClass('is-invalid');
		},
		unhighlight: function (element) {
			$(element).removeClass('is-invalid');
		}
	});

	$.validator.addMethod(
		"regex",
		function (value, element, regexp) {
			var re = new RegExp(regexp);
			var result = this.optional(element) || re.test(value);
			// alert(result);
			return result;
		},
		"Please check your input."
	);

	$.validator.addMethod(
		'nowhitespace',
		function (value, element) {
			return this.optional(element) || value.trim() != ''
		},
		'No white space.'
	);

	$.validator.addMethod(
		'stringarrayitemmaxlength',
		function (value, element, length) {
			var array = value.split(';');
			for (var i = 0; i < array.length; i++) {
				if (array[i].trim().length > length) {
					return false;
				}
			}
			return true;
		},
		'Length of word too long.'
	);

	/* validation for book */
	$('#book-form').validate({
		// errorClass: 'field-validation-error',
		// validClass: 'field-validation-valid',
		// errorPlacement: function (error, element) {
		// 	if (element.attr("name") == "stories")
		// 		error.insertAfter(element.parent());
		// 	else
		// 		error.insertAfter(element);
		// },
		// errorElement: 'span',
		rules: {
			number: {
				required: true,
				nowhitespace: true,
				regex: '^[0-9]+$'
			},
			title: {
				required: true,
				nowhitespace: true,
				maxlength: 200
			},
			category: {
				required: true,
				nowhitespace: true,
				maxlength: 100
			},
			// stories: {
			// 	maxlength: 10
			// },
			tags: {
				stringarrayitemmaxlength: 50
			}
		},
		messages: {
			number: {
				required: 'Bitte gib eine Nummer ein.',
				nowhitespace: 'Bitte gib eine Nummer ein.',
				regex: 'Bitte gib eine Nummer ein.'
			},
			title: {
				required: 'Bitte gib einen Titel ein.',
				nowhitespace: 'Bitte gib einen Titel ein.',
				maxlength: 'Der Titel darf max. 200 Zeichen lang sein.'
			},
			category: {
				required: 'Bitte gib eine Reihe ein.',
				nowhitespace: 'Bitte gib eine Reihe ein.',
				maxlength: 'Die Reihe darf max. 100 Zeichen lang sein.'
			},
			tags: {
				stringarrayitemmaxlength: 'Ein Tag darf max. 50 Zeichen lang sein.'
			}
		}
	});

	$('#login-form').validate({
		// errorClass: 'field-validation-error',
		// validClass: 'field-validation-valid',
		// errorElement: 'span',
		rules: {
			username: {
				required: true,
				nowhitespace: true
			},
			password: {
				required: true,
				nowhitespace: true
			}
		},
		messages: {
			username: {
				required: 'Bitte gib einen Benutzernamen ein.',
				nowhitespace: 'Bitte gib einen Benutzernamen ein.'
			},
			password: {
				required: 'Bitte gib ein Passwort ein.',
				nowhitespace: 'Bitte gib ein Passwort ein.'
			}
		}
	});

	$('#category-move-form').validate({
		// errorClass: 'field-validation-error',
		// validClass: 'field-validation-valid',
		// errorElement: 'span',
		rules: {
			from: {
				required: true,
				nowhitespace: true,
				maxlength: 100
			},
			to: {
				required: true,
				nowhitespace: true,
				maxlength: 100
			}
		},
		messages: {
			from: {
				required: 'Bitte gib eine Reihe ein.',
				nowhitespace: 'Bitte gib eine Reihe ein.',
				maxlength: 'Die Reihe darf max. 100 Zeichen lang sein.'
			},
			to: {
				required: 'Bitte gib eine Reihe ein.',
				nowhitespace: 'Bitte gib eine Reihe ein.',
				maxlength: 'Die Reihe darf max. 100 Zeichen lang sein.'
			}
		}
	});
});