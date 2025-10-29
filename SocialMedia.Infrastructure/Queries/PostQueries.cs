using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Queries
{
    public static class PostQueries
    {
        public static string PostQuerySqlServer = @"
                        select Id, UserId, Date, Description, Imagen 
                        from post 
                        order by Date desc
                        OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;";
        public static string PostQueryMySQl = @"
                        select Id, UserId, Date, Description, Imagen 
                        from post 
                        order by Date desc
                        LIMIT @Limit
                    ";
        public static string PostComentadosUsuariosActivos = @"
                        SELECT 
                        p.Id AS PostId,
                        p.Description,
                        COUNT(c.Id) AS TotalComentarios
                    FROM Post p
                    JOIN Comment c ON p.Id = c.PostId
                    JOIN User u ON c.UserId = u.Id
                    WHERE u.IsActive = 1        
                    GROUP BY p.Id, p.Description
                    HAVING COUNT(c.Id) > 2
                    ORDER BY TotalComentarios DESC;            
                    ";

        public static string PostsConComentariosDeMenores = @"
                        SELECT 
                            p.Id AS postId,
                            p.Description AS description,
                            COUNT(c.Id) AS totalComentarios
                        FROM 
                            [dbo].[Post] p
                        JOIN 
                            [dbo].[Comment] c ON p.Id = c.PostId
                        JOIN 
                            [dbo].[User] u ON c.UserId = u.Id
                        WHERE 
                            FLOOR(DATEDIFF(DAY, u.DateOfBirth, GETDATE()) / 365.25) < 18
                        GROUP BY 
                            p.Id, p.Description
                        ORDER BY
                            totalComentarios DESC;
                        ";

        // --- NUEVAS CONSULTAS AÑADIDAS ---

        /// <summary>
        /// Usuarios activos que no han realizado ningún comentario.
        /// (Usa LEFT JOIN y WHERE IS NULL para encontrar usuarios sin coincidencias)
        /// </summary>
        public static string UsuariosActivosSinComentarios = @"
                        SELECT
                            u.FirstName,
                            u.LastName,
                            u.Email
                        FROM
                            [dbo].[User] u
                        LEFT JOIN
                            [dbo].[Comment] c ON u.Id = c.UserId
                        WHERE
                            u.IsActive = 1
                            AND c.Id IS NULL;
                        ";

        /// <summary>
        /// Comentarios realizados en los últimos 3 meses por usuarios mayores de 25 años.
        /// (Combina filtros de fecha y cálculo de edad)
        /// </summary>
        public static string ComentariosRecientesDeMayores = @"
                        SELECT
                            c.Id AS IdComment,
                            c.Description AS [Commment Description],
                            u.FirstName,
                            u.LastName,
                            FLOOR(DATEDIFF(DAY, u.DateOfBirth, GETDATE()) / 365.25) AS Edad
                        FROM
                            [dbo].[Comment] c
                        JOIN
                            [dbo].[User] u ON c.UserId = u.Id
                        WHERE
                            c.Date >= DATEADD(MONTH, -3, GETDATE())
                            AND FLOOR(DATEDIFF(DAY, u.DateOfBirth, GETDATE()) / 365.25) > 25;
                        ";

        /// <summary>
        /// Publicaciones que no han recibido comentarios de usuarios activos.
        /// (Usa NOT EXISTS para encontrar posts sin comentarios que cumplan la condición)
        /// </summary>
        public static string PostsSinComentariosActivos = @"
                        SELECT
                            p.Id AS IdPost,
                            p.Description AS [Post Description],
                            p.Date AS [Post Date]
                        FROM
                            [dbo].[Post] p
                        WHERE
                            NOT EXISTS (
                                SELECT 1
                                FROM [dbo].[Comment] c
                                JOIN [dbo].[User] u ON c.UserId = u.Id
                                WHERE c.PostId = p.Id AND u.IsActive = 1
                            );
                        ";

        /// <summary>
        /// Usuarios que han comentado en posts de al menos 3 usuarios diferentes.
        /// (Identifica usuarios que interactúan con diversos autores)
        /// </summary>
        public static string UsuariosConComentariosADiversosAutores = @"
                        SELECT
                            u_commenter.FirstName,
                            u_commenter.LastName,
                            COUNT(DISTINCT p.UserId) AS cantidadDeUsuariosDiferentes
                        FROM
                            [dbo].[Comment] c
                        JOIN
                            [dbo].[User] u_commenter ON c.UserId = u_commenter.Id
                        JOIN
                            [dbo].[Post] p ON c.PostId = p.Id
                        GROUP BY
                            u_commenter.Id, u_commenter.FirstName, u_commenter.LastName
                        HAVING
                            COUNT(DISTINCT p.UserId) >= 3;
                        ";
    }
}
