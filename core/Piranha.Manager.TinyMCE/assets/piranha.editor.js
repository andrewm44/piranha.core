/*global
    piranha, tinymce
*/

//
// Create a new inline editor
//
piranha.editor.addInline = function (id, toolbarId) {
    tinymce.init({
        selector: "#" + id,
        setup: function (editor) {
            editor.ui.registry.addButton('enhanceText', {
                text: 'Enhance',
                onAction: function () {
                    // Open a dialog when the button is clicked
                    editor.windowManager.open({
                        title: 'Enhance Text',  // Dialog title
                        body: {
                            type: 'panel',
                            items: [
                                {
                                    type: 'input',
                                    name: 'userInput',
                                    label: 'Desired Word Count',
                                }
                            ]
                        },
                        buttons: [
                            {
                                type: 'submit',
                                text: 'Submit',
                                primary: true
                            },
                            {
                                type: 'cancel',
                                text: 'Cancel'
                            }
                        ],
                        onSubmit: function (dialogApi) {
                            // Get the content of the textbox and the TinyMCE editor
                            const userInput = dialogApi.getData().userInput;
                            const editorContent = editor.getContent({ format: 'text' });

                            // Perform the API call with both the content and the textbox value
                            fetch('/api/content/enhanceText', {
                                method: 'POST',
                                headers: {
                                    'Content-Type': 'application/json',
                                },
                                body: JSON.stringify({
                                    text: editorContent,
                                    wordCount: userInput // Pass the textbox value to the 
                                }),
                            })
                                .then(response => response.json())
                                .then(data => {
                                    editor.setContent(data.enhancedText);  // Update editor with the enhanced text
                                })
                                .catch(error => {
                                    console.error('Error enhancing text:', error);
                                });

                            dialogApi.close(); // Close the dialog after submission
                        }
                    });
                }

            });
            editor.ui.registry.addButton('embed', {
                text: 'Embed',
                onAction: function () {
                    // Open a dialog when the button is clicked
                    editor.windowManager.open({
                        title: 'Add Embed Card',  // Dialog title
                        body: {
                            type: 'panel',
                            items: [
                                {
                                    type: 'input',
                                    name: 'userInput',
                                    label: 'Link Url',
                                }
                            ]
                        },
                        buttons: [
                            {
                                type: 'submit',
                                text: 'Submit',
                                primary: true
                            },
                            {
                                type: 'cancel',
                                text: 'Cancel'
                            }
                        ],
                        onSubmit: function (dialogApi) {
                            // Get the content of the textbox and the TinyMCE editor
                            const userInput = dialogApi.getData().userInput;
                         //   const editorContent = editor.getContent({ format: 'text' });

                            // Perform the API call with both the content and the textbox value
                            fetch('/api/content/embedCard?linkUrl=' + userInput, {
                                method: 'GET',
                                headers: {
                                    'Content-Type': 'text/html',
                                },
                            })
                                .then(response => response.text()) // Change to .text() for HTML response
                                .then(htmlContent => {
                                    editor.setContent(htmlContent); // Update editor with the HTML response
                                })
                                .catch(error => {
                                    console.error('Error creating embed:', error);
                                });

                            dialogApi.close(); // Close the dialog after submission
                        }
                    });
                }

            });
        },
        browser_spellcheck: true,
        fixed_toolbar_container: "#" + toolbarId,
        menubar: false,
        branding: false,
        statusbar: false,
        inline: true,
        convert_urls: false,
        plugins: [
            piranha.editorconfig.plugins
        ],
        width: "100%",
        autoresize_min_height: 0,
        toolbar: piranha.editorconfig.toolbar,
        extended_valid_elements: piranha.editorconfig.extended_valid_elements,
        block_formats: piranha.editorconfig.block_formats,
        style_formats: piranha.editorconfig.style_formats,
        file_picker_callback: function(callback, value, meta) {
            // Provide file and text for the link dialog
            if (meta.filetype == 'file') {
                piranha.mediapicker.openCurrentFolder(function (data) {
                    callback(data.publicUrl, { text: data.filename });
                }, null);
            }

            // Provide image and alt text for the image dialog
            if (meta.filetype == 'image') {
                piranha.mediapicker.openCurrentFolder(function (data) {
                    callback(data.publicUrl, { alt: "" });
                }, "image");
            }
        }
    });
    $("#" + id).parent().append("<a class='tiny-brand' href='https://www.tiny.cloud' target='tiny'>Powered by Tiny</a>");
};

//
// Remove the TinyMCE instance with the given id.
//
piranha.editor.remove = function (id) {
    tinymce.remove(tinymce.get(id));
    $("#" + id).parent().find('.tiny-brand').remove();
};
