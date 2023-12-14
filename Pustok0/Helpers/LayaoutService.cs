using Pustok0.Context;
using Pustok0.Models;

namespace Pustok0.Helpers
{
    public class LayaoutService
    {
        PustokDbContext _context { get; }

        public LayaoutService(PustokDbContext context)
        {
            _context = context;
        }
        public async Task<Setting> GetSettingsAsync()
        => await _context.Settings.FindAsync(1);
    }
}
