$.widget("custom.catcomplete", $.ui.autocomplete, {
	_renderMenu: function(ul, items) {
		var that = this, currentCategory = "";
		$.each(items, function(index, item) {
			if(item.category != currentCategory) {
				ul.append('<li class="ui-autocomplete-category">' + item.category + '</li>');
				currentCategory = item.category;
			}
			that._renderItemData(ul, item);
		});
	}
});

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
	/* autocomplete for search */
	$('#q').autocomplete({
		source: '/api/search/title',
		minLength: 3,
		select: function (event, ui) {
			if (ui.item) {
				$(event.target).val(ui.item.value);
			}
			$(event.target.form).submit();
		},
		open: function () {
			$(this).autocomplete("widget").width($(this).outerWidth() - 6);
		}
	});

	/* autocomplete for tags */
	$('.t').autocomplete({
		source: function (request, response) {
			$.getJSON('/api/search/tags', {
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
		},
		open: function () {
			$(this).autocomplete("widget").width($(this).outerWidth() - 6);
		}
	});

	/* autocomplete for categories */
	$('.c').autocomplete({
		source: '/api/search/categories',
		minLength: 0,
		select: function (event, ui) {
			if (ui.item) {
				$(event.target).val(ui.item.value);
			}
		}
	}).focus(function() {
		$(this).autocomplete("search", $(this).val());
	});

	/* image box */
	jbox_image = new jBox('Image');

	/* story management */
	var story_container = $('#story-container');
	var story_template =	'<div class="story">\
								<input class="input" type="text" name="stories" value="" placeholder="Inhalt" /> <span class="button-green story-ins" title="Eintrag darüber einfügen."><i class="material-icons material-icons-small">add</i></span> <span class="button-red story-rem" title="Eintrag entfernen."><i class="material-icons material-icons-small">remove</i></span>\
							</div>';

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

	/* validation */
	$.validator.addMethod(
	    "regex",
	    function (value, element, regexp) {
	    	return this.optional(element) || value.match(regexp);
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

	$('#book-form').validate({
		errorClass: 'field-validation-error',
		validClass: 'field-validation-valid',
		errorElement: 'span',
		rules: {
			number: {
				required: true,
				nowhitespace: true,
				regex: '[0-9]+'
			},
			title: {
				required: true,
				nowhitespace: true,
				maxlength: 100
			},
			category: {
				required: true,
				nowhitespace: true,
				maxlength: 100
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
				maxlength: 'Der Titel darf max. 100 Zeichen lang sein.'
			},
			category: {
				required: 'Bitte gib eine Kategorie ein.',
				nowhitespace: 'Bitte gib eine Kategorie ein.',
				maxlength: 'Die Kategorie darf max. 100 Zeichen lang sein.'
			}
		}
	});

	$('#login-form').validate({
		errorClass: 'field-validation-error',
		validClass: 'field-validation-valid',
		errorElement: 'span',
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
		errorClass: 'field-validation-error',
		validClass: 'field-validation-valid',
		errorElement: 'span',
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
				required: 'Bitte gib eine Kategorie ein.',
				nowhitespace: 'Bitte gib eine Kategorie ein.',
				maxlength: 'Die Kategorie darf max. 100 Zeichen lang sein.'
			},
			to: {
				required: 'Bitte gib eine Kategorie ein.',
				nowhitespace: 'Bitte gib eine Kategorie ein.',
				maxlength: 'Die Kategorie darf max. 100 Zeichen lang sein.'
			}
		}
	});
});