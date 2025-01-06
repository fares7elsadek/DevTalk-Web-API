namespace DevTalk.Domain.Helpers;

public static class EmailTemplate
{
    public static string ConfirmEmail(string Url)
    {
        return $@"
<!DOCTYPE html>
<html>
    <head>
        <meta charset='utf-8' />
        <meta http-equiv='x-ua-compatible' content='ie=edge' />
        <title>Email Confirmation - DevTalk</title>
        <meta name='viewport' content='width=device-width, initial-scale=1' />
        <style>
            @media screen {{
                @font-face {{
                    font-family: 'Inter';
                    font-style: normal;
                    font-weight: 400;
                    src: url('https://fonts.googleapis.com/css2?family=Inter:wght@400;600;700&display=swap');
                }}
            }}

            body {{
                margin: 0;
                padding: 0;
                background-color: #f9fafb;
                font-family: 'Inter', Arial, sans-serif;
            }}

            .email-wrapper {{
                width: 100%;
                background-color: #f9fafb;
                padding: 20px 0;
            }}

            .email-container {{
                max-width: 600px;
                margin: 0 auto;
                background-color: #ffffff;
                border-radius: 8px;
                overflow: hidden;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            }}

            .email-header {{
                background-color: #1a73e8;
                text-align: center;
                padding: 30px 20px;
            }}

            .email-header img {{
                width: 120px;
            }}

            .email-body {{
                padding: 40px 30px;
                text-align: center;
            }}

            .email-body h1 {{
                font-size: 24px;
                font-weight: 700;
                color: #333333;
                margin-bottom: 20px;
            }}

            .email-body p {{
                font-size: 16px;
                color: #555555;
                line-height: 1.5;
                margin-bottom: 30px;
            }}

            .email-button {{
                margin: 30px 0;
            }}

            .email-button a {{
                background-color: #1a73e8;
                color: #ffffff;
                text-decoration: none;
                font-size: 16px;
                font-weight: 600;
                padding: 12px 24px;
                border-radius: 6px;
                display: inline-block;
            }}

            .email-footer {{
                background-color: #f1f5f9;
                padding: 20px 30px;
                text-align: center;
                font-size: 14px;
                color: #777777;
            }}

            .email-footer p {{
                margin: 5px 0;
            }}

            .email-footer a {{
                color: #1a73e8;
                text-decoration: none;
            }}
        </style>
    </head>
    <body>
        <div class='email-wrapper'>
            <div class='email-container'>
                <!-- Header Section -->
                <div class='email-header'>
                    <img src='https://res.cloudinary.com/df6ylojjq/image/upload/v1736182255/devTalk_fxlk05.svg' alt='DevTalk Logo' />
                </div>

                <!-- Body Section -->
                <div class='email-body'>
                    <h1>Confirm Your Email Address</h1>
                    <p>
                        Thank you for signing up with <strong>DevTalk</strong>! Please confirm your email address by clicking the button below.
                    </p>
                    <div class='email-button'>
                        <a href='{Url}' target='_blank'>Confirm Email</a>
                    </div>
                    <p>
                        If the button above doesn't work, copy and paste the following link into your browser:
                    </p>
                    <p>
                        <a href='{Url}' target='_blank'>{Url}</a>
                    </p>
                </div>

                <!-- Footer Section -->
                <div class='email-footer'>
                    <p>You received this email because you signed up for DevTalk.</p>
                    <p>If you didn't request this email, you can safely ignore it.</p>
                    <p>&copy; {DateTime.Now.Year} DevTalk. All rights reserved.</p>
                    <p><a href='#'>Privacy Policy</a> | <a href='#'>Contact Us</a></p>
                </div>
            </div>
        </div>
    </body>
</html>
";


    }
}
