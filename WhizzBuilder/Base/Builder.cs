using System;

namespace WhizzBuilder.Base
{
    public static class Builder
    {
        public static string CreateFile(Action<File> fileAction)
        {
            var file = new File();
            fileAction(file);
            return file.Build();
        }
    }
}
