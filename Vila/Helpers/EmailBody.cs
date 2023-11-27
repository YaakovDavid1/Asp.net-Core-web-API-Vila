﻿namespace Vila.Helpers
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"<html>
                <head>
                </head>
                <body style=""margin:0;padding:0;font-family: Arial, Helvetica, sans-serif;"">
                    <div style=""height: auto;background: linear-gradient(to top, #c9c9ff  50%, #6e6ef6 90%) no-repeat;width:400px;padding:30px"">
                    <div>
                        <div>
                            <h1>Reset your Password</h1>
                            <hr>
                            <p>You're receiving this E-mail because you requested a password reset for your Lets Program account.</p>
                            <p> Please tap the button below to choose a new password.</p>
                            <a href=""http://localhost:4200/reset?email={email}&code={emailToken}"" target=""_blank"" style=""background:#0d6efd;padding:10px;border:none;
                            color:white;border-radius:4px;display:block;margin:0 auto;width:50%;text-align:center;text-decoration:none"">Reset Password</a><br>

                            <p> Kind Regards, <br><br>
                            Let's Program</p>
                        </div>
                        </div>
                    </div>
                    </body>
                </html>";

        }
    }
}
