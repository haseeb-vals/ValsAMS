/**
 * Form validation and submission handling for MultiSaaS user forms
 */

const FormValidator = {
    init: function() {
        this.setupFormValidation();
        this.setupFormSubmission();
    },

    setupFormValidation: function() {
        // Add custom validation methods
        if ($.validator) {
            // Add a custom method for password strength
            $.validator.addMethod("strongPassword", function(value, element) {
                return this.optional(element) || 
                    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/.test(value);
            }, "Password must be at least 8 characters and include uppercase, lowercase, number, and special character");

            // Add a custom method for phone number format
            $.validator.addMethod("phoneFormat", function(value, element) {
                return this.optional(element) || 
                    /^\+?[\d\s\-\(\)]{10,15}$/.test(value);
            }, "Please enter a valid phone number");
        }

        // Initialize validation for all forms with the 'needs-validation' class
        $('form.needs-validation').each(function() {
            $(this).validate({
                errorElement: 'div',
                errorClass: 'invalid-feedback',
                highlight: function(element) {
                    $(element).addClass('is-invalid').removeClass('is-valid');
                },
                unhighlight: function(element) {
                    $(element).addClass('is-valid').removeClass('is-invalid');
                },
                errorPlacement: function(error, element) {
                    error.insertAfter(element);
                }
            });
        });

        // Add specific validation rules for user settings forms
        $('.user-settings-form').each(function() {
            $(this).validate({
                rules: {
                    firstName: {
                        required: true,
                        minlength: 2
                    },
                    lastName: {
                        required: true,
                        minlength: 2
                    },
                    email: {
                        required: true,
                        email: true
                    },
                    username: {
                        required: true,
                        minlength: 4
                    },
                    currentPassword: {
                        required: function(element) {
                            return $('#newPassword').val().length > 0;
                        }
                    },
                    newPassword: {
                        strongPassword: function(element) {
                            return $(element).val().length > 0;
                        }
                    },
                    confirmPassword: {
                        equalTo: '#newPassword'
                    },
                    phone: {
                        phoneFormat: true
                    }
                },
                messages: {
                    firstName: {
                        required: "Please enter your first name",
                        minlength: "First name must be at least 2 characters"
                    },
                    lastName: {
                        required: "Please enter your last name",
                        minlength: "Last name must be at least 2 characters"
                    },
                    email: {
                        required: "Please enter your email address",
                        email: "Please enter a valid email address"
                    },
                    username: {
                        required: "Please enter a username",
                        minlength: "Username must be at least 4 characters"
                    },
                    currentPassword: {
                        required: "Current password is required to set a new password"
                    },
                    confirmPassword: {
                        equalTo: "Passwords do not match"
                    }
                },
                errorElement: 'div',
                errorClass: 'invalid-feedback',
                highlight: function(element) {
                    $(element).addClass('is-invalid').removeClass('is-valid');
                },
                unhighlight: function(element) {
                    $(element).addClass('is-valid').removeClass('is-invalid');
                },
                errorPlacement: function(error, element) {
                    error.insertAfter(element);
                }
            });
        });

        // Add specific validation rules for distributor forms
        $('.distributor-form, .subdistributor-form').each(function() {
            $(this).validate({
                rules: {
                    Name: {
                        required: true,
                        minlength: 3
                    },
                    Code: {
                        required: true,
                        minlength: 3
                    },
                    Email: {
                        required: true,
                        email: true
                    },
                    Phone: {
                        phoneFormat: true
                    },
                    Website: {
                        url: true
                    }
                },
                messages: {
                    Name: {
                        required: "Please enter the distributor name",
                        minlength: "Name must be at least 3 characters"
                    },
                    Code: {
                        required: "Please enter a distributor code",
                        minlength: "Code must be at least 3 characters"
                    },
                    Email: {
                        required: "Please enter an email address",
                        email: "Please enter a valid email address"
                    },
                    Website: {
                        url: "Please enter a valid URL (include http:// or https://)"
                    }
                },
                errorElement: 'div',
                errorClass: 'invalid-feedback',
                highlight: function(element) {
                    $(element).addClass('is-invalid').removeClass('is-valid');
                },
                unhighlight: function(element) {
                    $(element).addClass('is-valid').removeClass('is-invalid');
                },
                errorPlacement: function(error, element) {
                    error.insertAfter(element);
                }
            });
        });
    },

    setupFormSubmission: function() {
        // Handle form submissions with AJAX
        $('form.ajax-form').on('submit', function(e) {
            e.preventDefault();
            
            const $form = $(this);
            
            // Only proceed if the form is valid
            if (!$form.valid()) {
                return false;
            }
            
            // Show loading indicator
            const $submitBtn = $form.find('[type="submit"]');
            const originalBtnText = $submitBtn.html();
            $submitBtn.html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Saving...');
            $submitBtn.prop('disabled', true);
            
            // Collect form data
            const formData = $form.serialize();
            
            // Send AJAX request
            $.ajax({
                url: $form.attr('action'),
                type: $form.attr('method') || 'POST',
                data: formData,
                success: function(response) {
                    // Show success message
                    FormValidator.showAlert('success', 'Changes saved successfully!');
                    
                    // If there's a redirect URL in the response, redirect after a delay
                    if (response.redirectUrl) {
                        setTimeout(function() {
                            window.location.href = response.redirectUrl;
                        }, 1500);
                    }
                },
                error: function(xhr) {
                    // Show error message
                    let errorMessage = 'An error occurred while saving changes.';
                    if (xhr.responseJSON && xhr.responseJSON.message) {
                        errorMessage = xhr.responseJSON.message;
                    }
                    FormValidator.showAlert('danger', errorMessage);
                },
                complete: function() {
                    // Restore button state
                    $submitBtn.html(originalBtnText);
                    $submitBtn.prop('disabled', false);
                }
            });
            
            return false;
        });
    },
    
    showAlert: function(type, message) {
        // Create alert element
        const $alert = $(`<div class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>`);
        
        // Find alert container or create one
        let $alertContainer = $('.alert-container');
        if ($alertContainer.length === 0) {
            $alertContainer = $('<div class="alert-container position-fixed top-0 end-0 p-3" style="z-index: 1050;"></div>');
            $('body').append($alertContainer);
        }
        
        // Add alert to container
        $alertContainer.append($alert);
        
        // Auto-dismiss after 5 seconds
        setTimeout(function() {
            $alert.alert('close');
        }, 5000);
    }
};

// Initialize when DOM is ready
$(document).ready(function() {
    FormValidator.init();
});