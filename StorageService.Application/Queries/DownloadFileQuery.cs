using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageService.Application.Queries
{
    public record DownloadFileQuery(string Id) : IRequest<(string Filename, byte[] Content)>;

}
