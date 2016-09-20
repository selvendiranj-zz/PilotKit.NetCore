using PilotKit.Domain.Interfaces.Common;
using PilotKit.Domain.Model.Common;
using PilotKit.Repository.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Domain.Services.Common
{
    public class LayoutService : DomainServiceBase, ILayoutService
    {
        private ILayoutRepository repository;

        public LayoutService(ILayoutRepository repository)
        {
            this.repository = repository;
        }

        public IList<PilotKitMenu> GetPilotKitMenu(string category)
        {
            IList<PilotKitMenu> mainMenu = new List<PilotKitMenu>();

            mainMenu = this.repository.GetPilotKitMenu(category);

            foreach (var menu in mainMenu)
            {
                CreateSubMenu(menu);
            }

            return mainMenu;
        }

        private void CreateSubMenu(PilotKitMenu menu)
        {
            IList<PilotKitMenu> subMenu = new List<PilotKitMenu>();
            menu.SubPilotKitMenu = new List<PilotKitMenu>();
            subMenu = this.repository.GetPilotKitMenu(menu.ID);

            foreach (var Menu in subMenu)
            {
                menu.SubPilotKitMenu.Add(Menu);

                if (Menu != null)
                {
                    CreateSubMenu(Menu);
                }
            }
        }
    }
}
